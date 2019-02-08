using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HWService
{
    static class HWCode
    {
        public static bool IsServiceStopping { get; set; } = false;

        public static void LogException(Exception ex)
        {
            Exception e = ex;
            while (e != null)
            {
                EventLog.WriteEntry("HWService", $"{e.GetType().FullName}\n{e.Message}\n{e.StackTrace}", EventLogEntryType.Error);
                e = e.InnerException;
            }
        }

        public static void HWServiceTask()
        {
            try
            {
                while (!IsServiceStopping)
                {
                    EventLog.WriteEntry("HWService", $"The thread has started successfully!", EventLogEntryType.Information);

                    JsonDeserialization jsonDeserialization = new JsonDeserialization();
                    jsonDeserialization.textFileJson();
                    jsonDeserialization.linkImgFileJson();
                    jsonDeserialization.linksFileJson();

                    DatabaseEntities dataBase = new DatabaseEntities();

                    foreach (var text in jsonDeserialization.dictionaryOfText)
                    {
                        textVK[] oldText = dataBase.textVK.Select(el => el).Where(el => (el.index.Equals(text.Key))).ToArray();
                        if (oldText.Length > 0)
                        {
                            EventLog.WriteEntry("HWService", $"The text from the post {text.Key} is already exists", EventLogEntryType.Warning);
                            continue;
                        }

                        textVK textVKTable = new textVK();
                        textVKTable.index = text.Key;
                        foreach (var txt in text.Value)
                        {
                            textVKTable.text = txt;
                            dataBase.textVK.Add(textVKTable);
                            EventLog.WriteEntry("HWService", $"Text from the post {text.Key} was successfully written", EventLogEntryType.SuccessAudit);
                            dataBase.SaveChanges();
                        }
                    }

                    foreach (var image in jsonDeserialization.dictionaryOfLinksImg)
                    {
                        imgVK[] oldTextCount = dataBase.imgVK.Select(el => el).Where(el => (el.index.Equals(image.Key))).ToArray();
                        if (oldTextCount.Length > 0)
                        {
                            EventLog.WriteEntry("HWService", $"Image from the post {image.Key} is already exists", EventLogEntryType.Warning);
                            continue;
                        }

                        imgVK imgVKTable = new imgVK();
                        if (image.Key == imgVKTable.index)
                        {
                            foreach (var img in image.Value)
                            {
                                if (imgVKTable.img != img)
                                {
                                    imgVKTable.index = image.Key;
                                    imgVKTable.img = img;
                                    dataBase.imgVK.Add(imgVKTable);
                                    EventLog.WriteEntry("HWService", $"Image from the post {image.Key} was successfully written", EventLogEntryType.SuccessAudit);
                                    dataBase.SaveChanges();
                                }
                            }
                        }
                        else
                        {
                            imgVKTable.index = image.Key;
                            foreach (var img in image.Value)
                            {
                                imgVKTable.img = img;
                                dataBase.imgVK.Add(imgVKTable);
                                EventLog.WriteEntry("HWService", $"Image from the post {image.Key} was successfully written", EventLogEntryType.SuccessAudit);
                                dataBase.SaveChanges();
                            }
                        }
                    }

                    foreach (var link in jsonDeserialization.dictionaryOfLinks)
                    {
                        linkVK[] oldTextCount = dataBase.linkVK.Select(el => el).Where(el => (el.index.Equals(link.Key))).ToArray();
                        if (oldTextCount.Length > 0)
                        {
                            EventLog.WriteEntry("HWService", $"Link from the post {link.Key} is already exists", EventLogEntryType.Warning);
                            continue;
                        }

                        linkVK linksVKTable = new linkVK();
                        if (link.Key == linksVKTable.index)
                        {
                            foreach (var lnk in link.Value)
                            {
                                if (linksVKTable.link != lnk)
                                {
                                    linksVKTable.index = link.Key;
                                    linksVKTable.link = lnk;
                                    dataBase.linkVK.Add(linksVKTable);
                                    dataBase.SaveChanges();
                                }
                            }
                        }
                        else
                        {
                            linksVKTable.index = link.Key;
                            foreach (var lnk in link.Value)
                            {
                                linksVKTable.link = lnk;
                                dataBase.linkVK.Add(linksVKTable);
                                dataBase.SaveChanges();
                            }
                        }
                    }

                    Thread.Sleep(9000);
                }
            }
            catch (AggregateException ex)
            {
                foreach (Exception e in ex.InnerExceptions)
                {
                    LogException(e);
                }
                IsServiceStopping = true;
            }
            catch (Exception e)
            {
                LogException(e);
                IsServiceStopping = true;
            }
        }
    }
}
