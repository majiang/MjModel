using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.IO;

namespace MjServer
{
    public class MjLogger
    {
        bool isDebug = false;
        Logger logger;
        DateTime logStartTime;

        public MjLogger()
        {
            if (isDebug)
            {
                logger = new DebugConsoleLogger();
            }
            else
            {
                logger = new FileLogger();
            }

            logStartTime = DateTime.Now;
        }

        public void Log(string msg)
        {
            logger.Log(msg);
        }

        public void OutPutFile()
        {
            logger.OutPutFile(logStartTime);
        }
        public void OutPutErrorFile()
        {
            logger.OutPutErrorFile(logStartTime);
        }
    }

    interface Logger
    {
        void Log(string msg);
        void OutPutFile(DateTime logStartTime);
        void OutPutErrorFile(DateTime logStartTime);
    }

    class DebugConsoleLogger : Logger
    {
        public void Log(string msg)
        {
            Debug.WriteLine(msg);
        }

        public void OutPutFile(DateTime logStartTime)
        {

        }
        public void OutPutErrorFile(DateTime logStartTime)
        {

        }
    }

    class FileLogger : Logger
    {
        List<string> msgList = new List<string>();
        public void Log(string msg)
        {
            msgList.Add(msg);
        }

        public void OutPutFile(DateTime logStartTime)
        {
            var targetFolder = MakeTargetDirectory(logStartTime);

            var outputFilePath = targetFolder + "/" + logStartTime.ToString("yyyy_MM_dd_hh_mm_ss_") + Guid.NewGuid()+ ".mjson";

            WriteFile(outputFilePath);
        }

        public void OutPutErrorFile(DateTime logStartTime)
        {
            var targetFolder = MakeTargetDirectory(logStartTime);

            var outputErrorFilePath = targetFolder + "/" + logStartTime.ToString("yyyy_MM_dd_hh_mm_ss_") + Guid.NewGuid() +"_Error"+ ".mjson";

            WriteFile(outputErrorFilePath);
        }

        string MakeTargetDirectory(DateTime logStartTime)
        {
            var outputfolder = "./log/" + logStartTime.ToString("yyyy") + "/" + logStartTime.ToString("MM");
            if (Directory.Exists(outputfolder) == false)
            {
                Directory.CreateDirectory(outputfolder);
            }
            return outputfolder;
        }
        void WriteFile(string outputFilePath)
        {
            using (StreamWriter sw = new StreamWriter(outputFilePath, false, Encoding.GetEncoding("utf-8")))
            {
                try
                {
                    msgList.ForEach(e => sw.WriteLine(e));
                    sw.Close();
                }
                catch (Exception e)
                {
                    Debug.WriteLine(e.Message);
                }
            }

        }
    }

}
