using System;
using DG.Tweening;
using Moon.Asyncs.Internal;

namespace Moon.Asyncs
{
    /// <summary>
    /// Extension for tween commands in async chains
    /// </summary>
    public static class TweenExtension
    {
        /// <summary>
        /// Add tween to execution queue
        /// </summary>
        /// <param name="chain">This chain</param>
        /// <param name="command">Tween command</param>
        /// <returns>This chain</returns>
        public static AsyncChain AddTween(this AsyncChain chain, Func<Tween> command)
        {
            return chain.AddCommand(new AsyncCommandTween(command));
        }

        /// <summary>
        /// Join tween to execution in paralel
        /// </summary>
        /// <param name="chain">This chain</param>
        /// <param name="command">Tween command</param>
        /// <returns>This chain</returns>
        public static AsyncChain JoinTween(this AsyncChain chain, Func<Tween> command)
        {
            return chain.JoinCommand(new AsyncCommandTween(command));
        }

        /// <summary>
        /// Add tween to execution queue
        /// </summary>
        /// <typeparam name="TArgs">Type of actions parameter</typeparam>
        /// <param name="chain">This chain</param>
        /// <param name="command">Tween command</param>
        /// <param name="args">Parameter for tween</param>
        /// <returns>This chain</returns>
        public static AsyncChain AddTween<TArgs>(this AsyncChain chain, Func<TArgs, Tween> command, TArgs args)
        {
            return chain.AddCommand(new AsyncCommandTweenArgs<TArgs>(command, args));
        }

        /// <summary>
        /// Join tween to execution in paralel
        /// </summary>
        /// <typeparam name="TArgs">Type of actions parameter</typeparam>
        /// <param name="chain">This chain</param>
        /// <param name="command">Tween command</param>
        /// <param name="args">Parameter for tween</param>
        /// <returns>This chain</returns>
        public static AsyncChain JoinTween<TArgs>(this AsyncChain chain, Func<TArgs, Tween> command, TArgs args)
        {
            return chain.JoinCommand(new AsyncCommandTweenArgs<TArgs>(command, args));
        }

        /// <summary>
        /// Add tween to execution queue
        /// </summary>
        /// <typeparam name="TArgs1">Type of actions 1st parameter</typeparam>
        /// <typeparam name="TArgs2">Type of actions 2nd parameter</typeparam>
        /// <param name="chain">This chain</param>
        /// <param name="command">Tween command</param>
        /// <param name="args1">1st parameter for action</param>
        /// <param name="args2">2nd parameter for action</param>
        /// <returns>This chain</returns>
        public static AsyncChain AddTween<TArgs1, TArgs2>(this AsyncChain chain, Func<TArgs1, TArgs2, Tween> command, TArgs1 args1, TArgs2 args2)
        {
            return chain.AddCommand(new AsyncCommandTweenArgs2<TArgs1, TArgs2>(command, args1, args2));
        }

        /// <summary>
        /// Join tween to execution in paralel
        /// </summary>
        /// <typeparam name="TArgs1">Type of actions 1st parameter</typeparam>
        /// <typeparam name="TArgs2">Type of actions 2nd parameter</typeparam>
        /// <param name="chain">This chain</param>
        /// <param name="command">Tween command</param>
        /// <param name="args1">1st parameter for action</param>
        /// <param name="args2">2nd parameter for action</param>
        /// <returns>This chain</returns>
        public static AsyncChain JoinTween<TArgs1, TArgs2>(this AsyncChain chain, Func<TArgs1, TArgs2, Tween> command, TArgs1 args1, TArgs2 args2)
        {
            return chain.JoinCommand(new AsyncCommandTweenArgs2<TArgs1, TArgs2>(command, args1, args2));
        }

        /// <summary>
        /// Add tween to execution queue
        /// </summary>
        /// <typeparam name="TArgs1">Type of actions 1st parameter</typeparam>
        /// <typeparam name="TArgs2">Type of actions 2nd parameter</typeparam>
        /// <typeparam name="TArgs3">Type of actions 3rd parameter</typeparam>
        /// <param name="chain">This chain</param>
        /// <param name="command">Tween command</param>
        /// <param name="args1">1st parameter for action</param>
        /// <param name="args2">2nd parameter for action</param>
        /// <param name="args3">3rd parameter for action</param>
        /// <returns>This chain</returns>
        public static AsyncChain AddTween<TArgs1, TArgs2, TArgs3>(this AsyncChain chain, Func<TArgs1, TArgs2, TArgs3, Tween> command, TArgs1 args1, TArgs2 args2, TArgs3 args3)
        {
            return chain.AddCommand(new AsyncCommandTweenArgs3<TArgs1, TArgs2, TArgs3>(command, args1, args2, args3));
        }

        /// <summary>
        /// Join tween to execution in paralel
        /// </summary>
        /// <typeparam name="TArgs1">Type of actions 1st parameter</typeparam>
        /// <typeparam name="TArgs2">Type of actions 2nd parameter</typeparam>
        /// <typeparam name="TArgs3">Type of actions 3rd parameter</typeparam>
        /// <param name="chain">This chain</param>
        /// <param name="command">Tween command</param>
        /// <param name="args1">1st parameter for action</param>
        /// <param name="args2">2nd parameter for action</param>
        /// <param name="args3">3rd parameter for action</param>
        /// <returns>This chain</returns>
        public static AsyncChain JoinTween<TArgs1, TArgs2, TArgs3>(this AsyncChain chain, Func<TArgs1, TArgs2, TArgs3, Tween> command, TArgs1 args1, TArgs2 args2, TArgs3 args3)
        {
            return chain.JoinCommand(new AsyncCommandTweenArgs3<TArgs1, TArgs2, TArgs3>(command, args1, args2, args3));
        }
    }
}
