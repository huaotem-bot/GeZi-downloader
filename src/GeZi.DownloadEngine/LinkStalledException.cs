using System;

namespace GeZi.DownloadEngine
{
    /// <summary>
    /// 「数据不再进来、但也没人报错」—— 停滞看门狗在连续若干分钟一个字节都没落盘时抛出。
    ///
    /// <para>
    /// 🚨 为什么单独建一个类型（2026-11-15）：
    /// 这类卡死的**外部表现不是 4xx**，而是连接被服务端静静关掉 / 分片请求被反复拒绝，
    /// 于是 <c>ClassifyLinkError</c> 认不出来，直链自愈机制全程不触发，
    /// 任务就永远停在 99.9%：不报成功、不报失败、连接也不释放。
    /// </para>
    /// <para>
    /// 有了这个类型，<c>ClassifyLinkError</c> 就能把它归到「值得重新取链」那一类，
    /// 交给既有的换链重试环处理 —— 换到新链后从断点继续，用户看到的只是「正在重新取链」。
    /// 只有当换链次数真的用完，才会作为失败上报。
    /// </para>
    /// </summary>
    public sealed class LinkStalledException : Exception
    {
        public LinkStalledException(string message) : base(message) { }

        public LinkStalledException(string message, Exception inner) : base(message, inner) { }
    }
}
