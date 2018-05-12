using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Athame.Core.Platform.Win32
{
    internal class Win32KeepAwakeContext : KeepAwakeContext
    {
        private EXECUTION_STATE state = EXECUTION_STATE.ES_CONTINUOUS;

        public override KeepAwakeRequirement GetKeepAwakeRequirement()
        {
            switch (state)
            {
                case EXECUTION_STATE.ES_AWAYMODE_REQUIRED:
                    throw new InvalidOperationException("ES_AWAYMODE_REQUIRED is not implemented.");
                    
                case EXECUTION_STATE.ES_CONTINUOUS:
                    return KeepAwakeRequirement.Clear;
                
                case EXECUTION_STATE.ES_DISPLAY_REQUIRED:
                case EXECUTION_STATE.ES_CONTINUOUS | EXECUTION_STATE.ES_DISPLAY_REQUIRED:
                    return KeepAwakeRequirement.Screen;

                case EXECUTION_STATE.ES_SYSTEM_REQUIRED:
                case EXECUTION_STATE.ES_CONTINUOUS | EXECUTION_STATE.ES_SYSTEM_REQUIRED:
                    return KeepAwakeRequirement.System;

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public override void SetKeepAwakeRequirement(KeepAwakeRequirement requirement)
        {
            base.SetKeepAwakeRequirement(requirement);
            EXECUTION_STATE newState;
            switch (requirement)
            {
                case KeepAwakeRequirement.Clear:
                    newState = EXECUTION_STATE.ES_CONTINUOUS;
                    break;

                case KeepAwakeRequirement.Screen:
                    newState = EXECUTION_STATE.ES_CONTINUOUS | EXECUTION_STATE.ES_DISPLAY_REQUIRED;
                    break;

                case KeepAwakeRequirement.System:
                    newState = EXECUTION_STATE.ES_CONTINUOUS | EXECUTION_STATE.ES_SYSTEM_REQUIRED; 
                    break;
                
                case KeepAwakeRequirement.Screen | KeepAwakeRequirement.System:
                    newState = EXECUTION_STATE.ES_CONTINUOUS | EXECUTION_STATE.ES_DISPLAY_REQUIRED | EXECUTION_STATE.ES_SYSTEM_REQUIRED;
                    break;

                default:
                    throw new ArgumentOutOfRangeException(nameof(requirement), requirement, null);
            }
            var result = Native.SetThreadExecutionState(newState);
            if (result == 0)
            {
                throw new InvalidOperationException("SetThreadExecutionState failed for some reason.");
            }
            state = (EXECUTION_STATE)result;
        }
    }
}
