﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LiteDB.Studio
{
    class TaskData
    {
        public const int RESULT_LIMIT = 1000;

        public int Id { get; set; }
        public bool Running { get; set; } = false;
        public string Sql { get; set; } = "";
        public string Collection { get; set; } = "";
        public List<BsonValue> Result { get; set; } = null;
        public string ExplainPlan { get; set; } = null;
        public bool LimitExceeded { get; set; }
        public Exception Exception { get; set; } = null;
        public TimeSpan Elapsed { get; set; } = TimeSpan.Zero;
        public BsonDocument Parameters { get; set; } = new BsonDocument();

        public Thread Thread { get; set; }

        public void ReadResult(BsonDataReader reader)
        {
            do
            {
                this.Result = new List<BsonValue>();
                this.LimitExceeded = false;
                this.Collection = reader.Collection;
                //this.ExplainPlan = reader.ExplainPlan;

                var index = 0;

                while (reader.Read())
                {
                    if (index++ >= RESULT_LIMIT)
                    {
                        this.LimitExceeded = true;
                        break;
                    }

                    this.Result.Add(reader.Current);
                }
            }
            while (reader.NextResult());
        }
    }
}
