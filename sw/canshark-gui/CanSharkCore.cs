﻿using Analysis;
using canshark;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public static class CanSharkCore
{
    public static ConcurrentQueue<CanMessage> InputQueue = new ConcurrentQueue<CanMessage>();
    public static ConcurrentBag<IAnalyzer> Analyzers = new ConcurrentBag<IAnalyzer>();

    public static void Analyze()
    {
        foreach (IAnalyzer anal in Analyzers)
            if (anal.IsRunning)
                return;

        CanMessage[] msgs = new CanMessage[InputQueue.Count];

        for (int i = 0; i < msgs.Length; i++)
            if (!InputQueue.TryDequeue(out msgs[i]))
            {
                Array.Resize(ref msgs, i - 1);
                break;
            }

        foreach(IAnalyzer anal in Analyzers)
            anal.Analyze(msgs);

    }
}
