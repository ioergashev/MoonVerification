using System;
using System.Collections;
using System.Diagnostics;
using System.Threading.Tasks;
using Moon.Asyncs.Internal;
using Moon.Asyncs.Internal.Commands;
using UnityEngine;

namespace Moon.Asyncs
{

    /// <summary>
    /// Async commands queue
    /// </summary>
    public class AsyncChain : AsyncState
    {
        private readonly PlannerSynchronizationContext _context;
        private AsyncCommandCollection _actualCommand;
        private readonly AsyncCommandsQueue _commands = new AsyncCommandsQueue();
        internal AsyncLayer layer;
        public string id;
        public string stackTraceString;

        private void CreateStackTrace()
        {
#if DEBUG
            var stackTrace = new StackTrace(2);
            stackTraceString = stackTrace.ToString();
#endif
        }
        
        internal AsyncChain(PlannerSynchronizationContext context)
        {
            _context = context;
            SetLayer(AsyncLayer.Game);
            CreateStackTrace();
        }
        
        /// <summary>
        /// Mark this chain as cut-scene
        /// </summary>
        /// <returns>This chain</returns>
        public AsyncChain AsCutScene()
        {
            SetLayer(AsyncLayer.CutScene);
            return this;
        }

        /// <summary>
        /// Sets identifier for this chain
        /// No check for uniqueness
        /// </summary>
        /// <param name="id">Identifier for this chain</param>
        /// <returns>This chain</returns>
        public AsyncChain WithId(string id)
        {
            this.id = id;
            return this;
        }
        
        internal AsyncChain AddCommand(AsyncCommand command)
        {
            _commands.Enqueue(new AsyncCommandCollection(command));
            return this;
        }

        internal AsyncChain JoinCommand(AsyncCommand command)
        {
            var last = _commands.Last;
            if (last == null)
            {
                _commands.Enqueue(new AsyncCommandCollection(command));
            }
            else
            {
                last.commands.Add(command);
            }
            return this;
        }
        
        private AsyncChain AddState(AsyncState state)
        {
            return AddCommand(new AsyncCommandState(state));
        }

        private AsyncChain JoinState(AsyncState state)
        {
            return JoinCommand(new AsyncCommandState(state));
        }
        
        /// <summary>
        /// Add completed async action to 
        /// </summary>
        /// <returns>Completed async action</returns>
        public AsyncChain AddEmpty()
        {
            return AddState(Empty());
        }

        /// <summary>
        /// Add async action wich never complete
        /// </summary>
        /// <returns>Async action wich never complete</returns>
        public AsyncChain AddNever()
        {
            return AddState(Never());
        }

        /// <summary>
        /// Add coroutine to chain
        /// </summary>
        /// <param name="coroutine"></param>
        /// <returns>This chain</returns>
        public AsyncChain AddCoroutine(IEnumerator coroutine)
        {
            return AddState(new AsyncStateCoroutine(coroutine));
        }

        /// <summary>
        /// Add coroutine to chain
        /// </summary>
        /// <param name="coroutine"></param>
        /// <returns>This chain</returns>
        public AsyncChain AddCoroutine(WaitForEndOfFrame coroutine)
        {
            return AddState(new AsyncStateCoroutine(coroutine));
        }

        /// <summary>
        /// Add coroutine to chain
        /// </summary>
        /// <param name="coroutine"></param>
        /// <returns>This chain</returns>
        public AsyncChain AddCoroutine(AsyncOperation coroutine)
        {
            return AddState(new AsyncStateCoroutine(coroutine));
        }

        /// <summary>
        /// Add action to execution queue
        /// </summary>
        /// <param name="action">Action to queue</param>
        /// <param name="attributes">Attributes for queued command</param>
        /// <returns>This chain</returns>
        public AsyncChain AddAction(Action action, CommandAttributes attributes = CommandAttributes.None)
        {
            return AddCommand(new AsyncCommandAction(action, attributes));
        }

        /// <summary>
        /// Add action to execution queue
        /// </summary>
        /// <typeparam name="TArgs">Type of actions parameter</typeparam>
        /// <param name="action">Action to queue</param>
        /// <param name="args">Parameter for action</param>
        /// <param name="attributes">Attributes for queued command</param>
        /// <returns>This chain</returns>
        public AsyncChain AddAction<TArgs>(Action<TArgs> action, TArgs args, CommandAttributes attributes = CommandAttributes.None)
        {
            return AddCommand(new AsyncCommandActionArgs<TArgs>(action, args, attributes));
        }

        /// <summary>
        /// Add action to execution queue
        /// </summary>
        /// <typeparam name="TArgs1">Type of actions 1st parameter</typeparam>
        /// <typeparam name="TArgs2">Type of actions 2nd parameter</typeparam>
        /// <param name="action">Action to queue</param>
        /// <param name="args1">1st parameter for action</param>
        /// <param name="args2">2nd parameter for action</param>
        /// <param name="attributes">Attributes for queued command</param>
        /// <returns>This chain</returns>
        public AsyncChain AddAction<TArgs1, TArgs2>(Action<TArgs1, TArgs2> action, TArgs1 args1, TArgs2 args2, CommandAttributes attributes = CommandAttributes.None)
        {
            return AddCommand(new AsyncCommandActionArgs2<TArgs1, TArgs2>(action, args1, args2, attributes));
        }

        /// <summary>
        /// Add action to execution queue
        /// </summary>
        /// <typeparam name="TArgs1">Type of actions 1st parameter</typeparam>
        /// <typeparam name="TArgs2">Type of actions 2nd parameter</typeparam>
        /// <typeparam name="TArgs3">Type of actions 3rd parameter</typeparam>
        /// <param name="action">Action to queue</param>
        /// <param name="args1">1st parameter for action</param>
        /// <param name="args2">2nd parameter for action</param>
        /// <param name="args3">3rd parameter for action</param>
        /// <param name="attributes"></param>
        /// <returns>This chain</returns>
        public AsyncChain AddAction<TArgs1, TArgs2, TArgs3>(Action<TArgs1, TArgs2, TArgs3> action, TArgs1 args1, TArgs2 args2, TArgs3 args3, CommandAttributes attributes = CommandAttributes.None)
        {
            return AddCommand(new AsyncCommandActionArgs3<TArgs1, TArgs2, TArgs3>(action, args1, args2, args3, attributes));
        }

        /// <summary>
        /// Add async action to execution queue
        /// </summary>
        /// <param name="command">Async action to queue</param>
        /// <returns>This chain</returns>
        public AsyncChain AddFunc(Func<AsyncState> action)
        {
            return AddCommand(new AsyncCommandFunc(action));
        }

        /// <summary>
        /// Join async action to execution in paralel
        /// </summary>
        /// <param name="command">Async action to queue</param>
        /// <returns>This chain</returns>
        public AsyncChain JoinFunc(Func<AsyncState> action)
        {
            return JoinCommand(new AsyncCommandFunc(action));
        }

        /// <summary>
        /// Add async action to execution queue
        /// </summary>
        /// <typeparam name="TArgs">Type of actions parameter</typeparam>
        /// <param name="command">Async action to queue</param>
        /// <param name="args">Parameter for action</param>
        /// <returns>This chain</returns>
        public AsyncChain AddFunc<TArgs>(Func<TArgs, AsyncState> action, TArgs args)
        {
            return AddCommand(new AsyncCommandFuncArgs<TArgs>(action, args));
        }

        /// <summary>
        /// Join async action to execution in paralel
        /// </summary>
        /// <typeparam name="TArgs">Type of actions parameter</typeparam>
        /// <param name="command">Async action to queue</param>
        /// <param name="args">Parameter for action</param>
        /// <returns>This chain</returns>
        public AsyncChain JoinFunc<TArgs>(Func<TArgs, AsyncState> action, TArgs args)
        {
            return JoinCommand(new AsyncCommandFuncArgs<TArgs>(action, args));
        }

        /// <summary>
        /// Add async action to execution queue
        /// </summary>
        /// <typeparam name="TArgs1">Type of actions 1st parameter</typeparam>
        /// <typeparam name="TArgs2">Type of actions 2nd parameter</typeparam>
        /// <param name="command">Async action to queue</param>
        /// <param name="args1">1st parameter for action</param>
        /// <param name="args2">2nd parameter for action</param>
        /// <returns>This chain</returns>
        public AsyncChain AddFunc<TArgs1, TArgs2>(Func<TArgs1, TArgs2, AsyncState> action, TArgs1 args1, TArgs2 args2)
        {
            return AddCommand(new AsyncCommandFuncArgs2<TArgs1, TArgs2>(action, args1, args2));
        }

        /// <summary>
        /// Add async action to execution queue
        /// </summary>
        /// <typeparam name="TArgs1">Type of actions 1st parameter</typeparam>
        /// <typeparam name="TArgs2">Type of actions 2nd parameter</typeparam>
        /// <typeparam name="TArgs3">Type of actions 3rd parameter</typeparam>
        /// <param name="command">Async action to queue</param>
        /// <param name="args1">1st parameter for action</param>
        /// <param name="args2">2nd parameter for action</param>
        /// <param name="args3">3nd parameter for action</param>
        /// <returns>This chain</returns>
        public AsyncChain AddFunc<TArgs1, TArgs2, TArgs3>(Func<TArgs1, TArgs2, TArgs3, AsyncState> action, TArgs1 args1, TArgs2 args2, TArgs3 args3)
        {
            return AddCommand(new AsyncCommandFuncArgs3<TArgs1, TArgs2, TArgs3>(action, args1, args2, args3));
        }

        /// <summary>
        /// Join async action to execution in paralel
        /// </summary>
        /// <typeparam name="TArgs1">Type of actions 1st parameter</typeparam>
        /// <typeparam name="TArgs2">Type of actions 2nd parameter</typeparam>
        /// <param name="command">Async action to queue</param>
        /// <param name="args1">1st parameter for action</param>
        /// <param name="args2">2nd parameter for action</param>
        /// <returns>This chain</returns>
        public AsyncChain JoinFunc<TArgs1, TArgs2>(Func<TArgs1, TArgs2, AsyncState> action, TArgs1 args1, TArgs2 args2)
        {
            return JoinCommand(new AsyncCommandFuncArgs2<TArgs1, TArgs2>(action, args1, args2));
        }

        /// <summary>
        /// Add task to execution queue. 
        /// Note, terminating task commands not supported, so if this command has started, it will comlete any way
        /// In case chain terminating next commands will not start
        /// </summary>
        /// <param name="command">Async action to queue</param>
        /// <returns>This chain</returns>
        public AsyncChain AddTask(Func<Task> command)
        {
            return AddCommand(new AsyncCommandTask(command));
        }

        /// <summary>
        /// Join task to execution in paralel
        /// Note, terminating task commands not supported, so if this command has started, it will comlete any way
        /// In case chain terminating next commands will not start
        /// </summary>
        /// <param name="command">Async action to queue</param>
        /// <returns>This chain</returns>
        public AsyncChain JoinTask(Func<Task> command)
        {
            return JoinCommand(new AsyncCommandTask(command));
        }

        /// <summary>
        /// Add task to execution queue
        /// Note, terminating task commands not supported, so if this command has started, it will comlete any way
        /// In case chain terminating next commands will not start
        /// </summary>
        /// <typeparam name="TArgs">Type of actions parameter</typeparam>
        /// <param name="command">Async action to queue</param>
        /// <param name="args"></param>
        /// <returns>This chain</returns>
        public AsyncChain AddTask<TArgs>(Func<TArgs, Task> command, TArgs args)
        {
            return AddCommand(new AsyncCommandTaskArgs<TArgs>(command, args));
        }

        /// <summary>
        /// Join task to execution in paralel
        /// Note, terminating task commands not supported, so if this command has started, it will comlete any way
        /// In case chain terminating next commands will not start
        /// </summary>
        /// <typeparam name="TArgs">Type of actions parameter</typeparam>
        /// <param name="command">Async action to queue</param>
        /// <param name="args"></param>
        /// <returns>This chain</returns>
        public AsyncChain JoinTask<TArgs>(Func<TArgs, Task> command, TArgs args)
        {
            return JoinCommand(new AsyncCommandTaskArgs<TArgs>(command, args));
        }

        /// <summary>
        /// Add task to execution queue
        /// Note, terminating task commands not supported, so if this command has started, it will comlete any way
        /// In case chain terminating next commands will not start
        /// </summary>
        /// <typeparam name="TArgs1">Type of actions 1st parameter</typeparam>
        /// <typeparam name="TArgs2">Type of actions 2nd parameter</typeparam>
        /// <param name="command">Async action to queue</param>
        /// <param name="args1">1st parameter for action</param>
        /// <param name="args2">2nd parameter for action</param>
        /// <returns>This chain</returns>
        public AsyncChain AddTask<TArgs1, TArgs2>(Func<TArgs1, TArgs2, Task> command, TArgs1 args1, TArgs2 args2)
        {
            return AddCommand(new AsyncCommandTaskArgs2<TArgs1, TArgs2>(command, args1, args2));
        }

        /// <summary>
        /// Join task to execution in paralel
        /// Note, terminating task commands not supported, so if this command has started, it will comlete any way
        /// In case chain terminating next commands will not start
        /// </summary>
        /// <typeparam name="TArgs1">Type of actions 1st parameter</typeparam>
        /// <typeparam name="TArgs2">Type of actions 2nd parameter</typeparam>
        /// <param name="command">Async action to queue</param>
        /// <param name="args1">1st parameter for action</param>
        /// <param name="args2">2nd parameter for action</param>
        /// <returns>This chain</returns>
        public AsyncChain JoinTask<TArgs1, TArgs2>(Func<TArgs1, TArgs2, Task> command, TArgs1 args1, TArgs2 args2)
        {
            return JoinCommand(new AsyncCommandTaskArgs2<TArgs1, TArgs2>(command, args1, args2));
        }

        /// <summary>
        /// Add task to execution queue
        /// Note, terminating task commands not supported, so if this command has started, it will comlete any way
        /// In case chain terminating next commands will not start
        /// </summary>
        /// <typeparam name="TArgs1">Type of actions 1st parameter</typeparam>
        /// <typeparam name="TArgs2">Type of actions 2nd parameter</typeparam>
        /// <typeparam name="TArgs3">Type of actions 3rd parameter</typeparam>
        /// <param name="command">Async action to queue</param>
        /// <param name="args1">1st parameter for action</param>
        /// <param name="args2">2nd parameter for action</param>
        /// <param name="args3">3rd parameter for action</param>
        /// <returns>This chain</returns>
        public AsyncChain AddTask<TArgs1, TArgs2, TArgs3>(Func<TArgs1, TArgs2, TArgs3, Task> command, TArgs1 args1, TArgs2 args2, TArgs3 args3)
        {
            return AddCommand(new AsyncCommandTaskArgs3<TArgs1, TArgs2, TArgs3>(command, args1, args2, args3));
        }

        /// <summary>
        /// Join task to execution in paralel
        /// Note, terminating task commands not supported, so if this command has started, it will comlete any way
        /// In case chain terminating next commands will not start
        /// </summary>
        /// <typeparam name="TArgs1">Type of actions 1st parameter</typeparam>
        /// <typeparam name="TArgs2">Type of actions 2nd parameter</typeparam>
        /// <typeparam name="TArgs3">Type of actions 3rd parameter</typeparam>
        /// <param name="command">Async action to queue</param>
        /// <param name="args1">1st parameter for action</param>
        /// <param name="args2">2nd parameter for action</param>
        /// <param name="args3">3rd parameter for action</param>
        /// <returns>This chain</returns>
        public AsyncChain JoinTask<TArgs1, TArgs2, TArgs3>(Func<TArgs1, TArgs2, TArgs3, Task> command, TArgs1 args1, TArgs2 args2, TArgs3 args3)
        {
            return JoinCommand(new AsyncCommandTaskArgs3<TArgs1, TArgs2, TArgs3>(command, args1, args2, args3));
        }

        /// <summary>
        /// Add delay to execution queue
        /// </summary>
        /// <param name="time">Seconds to wait before next command</param>
        /// <param name="independent"></param>
        /// <returns>This chain</returns>
        public AsyncChain AddTimeout(float time, bool independent = false)
        {
            AddState(FromTimer(time, independent));
            return this;
        }

        public AsyncChain SkipFrames(int count)
        {
            AddState(FromFrames(count));
            return this;
        }

        /// <summary>
        /// Join delay to run in paralel 
        /// </summary>
        /// <param name="time">Seconds to wait before next command</param>
        /// <param name="independent"></param>
        /// <returns>This chain</returns>
        public AsyncChain JoinTimeout(float time, bool independent = false)
        {
            JoinState(FromTimer(time, independent));
            return this;
        }

        /// <summary>
        /// Add callback to execution queue
        /// this callback handle async action finish
        /// </summary>
        /// <param name="awaitFunc">Callback to handle async action finish</param>
        /// <returns>This chain</returns>
        public AsyncChain AddAwait(Action<AsyncStateInfo> awaitFunc)
        {
            return AddState(FromAwait(awaitFunc));
        }

        /// <summary>
        /// Add callback to execution queue
        /// this callback handle async action finish
        /// </summary>
        /// <param name="awaitFunc">Callback to handle async action finish</param>
        /// <param name="args">Parameter for action</param>
        /// <returns>This chain</returns>
        public AsyncChain AddAwait<TArgs>(Action<TArgs, AsyncStateInfo> awaitFunc, TArgs args)
        {
            return AddState(FromAwait(awaitFunc, args));
        }
        
        /// <summary>
        /// Join callback to paralel awaiting
        /// this callback handle async action finish
        /// Next command will start whell all joined action complete
        /// </summary>
        /// <param name="awaitFunc">Callback to handle async action finish</param>
        /// <returns>This chain</returns>
        public AsyncChain JoinAwait(Action<AsyncStateInfo> awaitFunc)
        {
            return JoinState(FromAwait(awaitFunc));
        }

        /// <summary>
        /// Add unity async action to execution queue
        /// </summary>
        /// <param name="asyncFunc">Unity async action to queue</param>
        /// <returns>This chain</returns>
        public AsyncChain AddAsyncOperation(Func<AsyncOperation> asyncFunc)
        {
            AddState(FromAsyncOperation(asyncFunc));
            return this;
        }

        /// <summary>
        /// Join unity async action to execution in paralel
        /// Next command will start whell all joined action complete
        /// </summary>
        /// <param name="asyncFunc">Unity async action to queue</param>
        /// <returns>This chain</returns>
        public AsyncChain JoinAsyncOperation(Func<AsyncOperation> asyncFunc)
        {
            JoinState(FromAsyncOperation(asyncFunc));
            return this;
        }

        /// <summary>
        /// Add unity async action to execution queue
        /// </summary>
        /// <typeparam name="TArgs">Type of actions parameter</typeparam>
        /// <param name="asyncFunc">Unity async action to queue</param>
        /// <param name="args">Parameter for action</param>
        /// <returns>This chain</returns>
        public AsyncChain AddAsyncOperation<TArgs>(Func<TArgs, AsyncOperation> asyncFunc, TArgs args)
        {
            AddState(FromAsyncOperation(asyncFunc, args));
            return this;
        }

        /// <summary>
        /// Join unity async action to execution in paralel
        /// Next command will start whell all joined action complete
        /// </summary>
        /// <typeparam name="TArgs">Type of actions parameter</typeparam>
        /// <param name="asyncFunc">Unity async action to queue</param>
        /// <param name="args">Parameter for action</param>
        /// <returns>This chain</returns>
        public AsyncChain JoinAsyncOperation<TArgs>(Func<TArgs, AsyncOperation> asyncFunc, TArgs args)
        {
            JoinState(FromAsyncOperation(asyncFunc, args));
            return this;
        }

        /// <summary>
        /// Invokes execution queue
        /// </summary>
        internal AsyncCommandResult UpdateChain()
        {
            while (true)
            {
                if (_actualCommand == null)
                {
                    isComplete = _commands.Count == 0;
                    if (isComplete) return AsyncCommandResult.Ok;
                    _actualCommand = _commands.Dequeue();
                    AsyncCommandResult startResult = _actualCommand.Start();
                    if (startResult != AsyncCommandResult.Ok)
                    {
                        return startResult;
                    }
                }

                if (_actualCommand == null)
                {
                    // chain was terminated
                    return AsyncCommandResult.Ok;
                }

                _actualCommand.CheckNullStates();
                AsyncCommandResult updateResult = _actualCommand.UpdateStates();
                _actualCommand.CheckAndReportComplete();
                
                if (updateResult != AsyncCommandResult.Ok)
                {
                    return updateResult;
                }

                if (_actualCommand.isComplete)
                {
                    _actualCommand = null;
                }
                else
                {
                    break;
                }
            }

            return AsyncCommandResult.Ok;
        }

        /// <summary>
        /// Dummy inmplementation of base class
        /// </summary>
        public override void Update()
        {
        }

        /// <summary>
        /// Terminates chains execution
        /// Note, terminating task commands not supported, so if this command has started, it will comlete any way
        /// In case chain terminating next commands will not start
        /// </summary>
        public override void Terminate()
        {
            if (_actualCommand != null)
            {
                _actualCommand.Terminate();
                _actualCommand = null;
            };
            while (_commands.Count != 0)
            {
                var command = _commands.Dequeue();
                command.ReportChainTerminated();
            }
        }

        private void SetId(string id)
        {
        }

        protected void SetLoadingLayer()
        {
            SetLayer(AsyncLayer.Loading);
        }

        private void SetLayer(AsyncLayer newlayer)
        {
            if (layer == newlayer)
            {
                return;
            }

            var newlayerInstance = _context.GetLayer(newlayer);
            var prevlayerInstance = _context.GetLayer(layer);

            layer = newlayer;
            newlayerInstance.Inc();
            prevlayerInstance.Dec();
        }

        private AsyncChain AddChain(AsyncChain chain)
        {
            AddState(chain);
            return this;
        }
    }

}
