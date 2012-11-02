using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CoreSQL.Entities
{
    partial class AikeEntities
    {
        static private AikeEntities _instance = null;
        static public AikeEntities Instance() {
            if (AikeEntities._instance == null)
				AikeEntities._instance = CoreSQL.Helpers.EntityHelper<AikeEntities>.CreateInstance();
            return AikeEntities._instance;
        }
    }
}
