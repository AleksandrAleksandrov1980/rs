       public class CCommand
        {
            public enum enCommand
            {
                UN_INIT,
                GRAM_START,
                GRAM_STOP,
                GRAM_STATE,
                GRAM_TIMER
            };

            public static string GetCommandName(enCommand enCom)
            {
                switch (enCom)
                {
                    case enCommand.UN_INIT: return "UN_INIT";
                    case enCommand.GRAM_START: return "GRAM_START";
                    case enCommand.GRAM_STOP: return "GRAM_STOP";
                    case enCommand.GRAM_STATE: return "GRAM_STATE";
                    case enCommand.GRAM_TIMER: return "GRAM_TIMER";
                }
                return "UnKnownCom";
            }

            public int nId { get; set; } = -1;
            public enCommand enCom { get; set; } = enCommand.UN_INIT;
            public string strParams { get; set; } = "";
            public string strErr = "";
        }