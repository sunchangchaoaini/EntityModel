﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using Newtonsoft.Json;

namespace Agebull.Common
{
    /// <summary>
    /// 多生产者单消费者的同步列表（线程安全）
    /// </summary>
    /// <typeparam name="T">泛型对象</typeparam>
    /// <remarks>
    /// 1 内部使用信号量
    /// 2 用于多生产者单消费者的场景
    /// 3 使用双队列，以防止错误时无法还原
    /// </remarks>
    [JsonObject(MemberSerialization.OptIn)]
    public class MulitToOneQueue<T>
    {
        /// <summary>
        /// 内部队列
        /// </summary>
        [JsonProperty]
        public Queue<T> Queue { get; } = new Queue<T>();

        /// <summary>
        /// 正在处理
        /// </summary>
        [JsonProperty]
        public Queue<T> Doing { get; } = new Queue<T>();

        /// <summary>
        /// 用于同步的信号量
        /// </summary>
        private Semaphore _semaphore = new Semaphore(0, Int32.MaxValue);

        /// <summary>
        /// 是否为空
        /// </summary>
        /// <returns></returns>
        public bool IsEmpty()
        {
            return Queue.Count == 0 && Doing.Count == 0;
        }



        /// <summary>
        /// 保存以备下次启动时使用
        /// </summary>
        public void Save(string file)
        {
            Queue<T> queue = new Queue<T>();
            lock (Doing)
                foreach (var doing in Doing.ToArray())
                    queue.Enqueue(doing);
            lock (Queue)
                foreach (var doing in Queue)
                    queue.Enqueue(doing);
            File.WriteAllText(file, JsonConvert.SerializeObject(queue));
        }

        /// <summary>
        /// 载入保存的内容
        /// </summary>
        /// <returns>队列对象</returns>
        public static MulitToOneQueue<T> Load(string file)
        {
            MulitToOneQueue<T> queue = new MulitToOneQueue<T>();
            if (!File.Exists(file))
                return queue;
            try
            {
                var json = File.ReadAllText(file);
                if (!string.IsNullOrWhiteSpace(json))
                {
                    var old = JsonConvert.DeserializeObject<Queue<T>>(json);
                    foreach (var doing in old)
                        queue.Doing.Enqueue(doing);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            return queue;
        }

        /// <summary>
        /// 加入队列
        /// </summary>
        /// <param name="t"></param>
        public void Push(T t)
        {
            lock (Queue)
            {
                Queue.Enqueue(t);
            }
            _semaphore.Release();
        }

        /// <summary>
        /// 开始处理队列内容
        /// </summary>
        /// <param name="t">返回内容（如果返回True)</param>
        /// <param name="waitMs">等待时长</param>
        public bool StartProcess(out T t, int waitMs)
        {
            if (Doing.Count > 0)//之前存在失败
            {
                lock (Doing)
                    t = Doing.Peek();
                return true;
            }
            if (!_semaphore.WaitOne(waitMs))
            {
                t = default(T);
                return false;
            }
            lock (Queue)
            {
                t = Queue.Dequeue();
            }
            Doing.Enqueue(t);
            return true;
        }

        /// <summary>
        /// 完成处理队列内容
        /// </summary>
        public void EndProcess()
        {
            lock (Doing)
                Doing.Dequeue();
        }
    }
}