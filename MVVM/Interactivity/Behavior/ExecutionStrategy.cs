using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyWPFUI.Controls
{
    /// <summary>
    /// 定义了一个CommandBehaviorBinding的Behavior接口
    /// </summary>
    public interface IExecutionStrategy
    {
        /// <summary>
        /// 定义了一个CommandBehaviorBinding行为
        /// </summary>
        CommandBehaviorBinding Behavior { get; set; }

        /// <summary>
        /// 执行方法
        /// </summary>
        /// <param name="parameter">参数</param>
        void Execute(object parameter);
    }

    /// <summary>
    /// 执行的command
    /// </summary>
    public class CommandExecutionStrategy : IExecutionStrategy
    {
        #region IExecutionStrategy Members
        /// <summary>
        /// 定义了一个CommandBehaviorBinding行为
        /// </summary>
        public CommandBehaviorBinding Behavior { get; set; }

        public void Execute(object parameter)
        {
            if (Behavior == null)
                throw new InvalidOperationException("Behavior property cannot be null when executing a strategy");

            if (Behavior.Command.CanExecute(Behavior.CommandParameter))
                Behavior.Command.Execute(Behavior.CommandParameter);
        }

        #endregion
    }

    /// <summary>
    /// 执行的方法
    /// </summary>
    public class ActionExecutionStrategy : IExecutionStrategy
    {

        #region IExecutionStrategy Members

        /// <summary>
        /// 定义了一个CommandBehaviorBinding行为
        /// </summary>
        public CommandBehaviorBinding Behavior { get; set; }

        public void Execute(object parameter)
        {
            Behavior.Action(parameter);
        }

        #endregion
    }
}
