using System;

namespace QuanticCollapse
{
    public interface IExternalBooster
    {
        public string BoosterId { get; }
        public void Execute(GridModel gridModel, Action<string, bool> confirmExecution);
    }
}