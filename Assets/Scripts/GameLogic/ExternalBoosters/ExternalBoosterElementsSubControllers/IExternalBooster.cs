using System;

namespace QuanticCollapse
{
    public interface IExternalBooster
    {
        public string BoosterId { get; }
        public void Execute(GridModel Model, Action<string, bool> ConfirmExecution);
    }
}