using GalaSoft.MvvmLight.Messaging;
using HMI_Master_KX7500.Initialize;
using HMI_Master_KX7500.MachinePath;
using HMI_Master_KX7500.Model.Language;
using HMI_Master_KX7500.Model.Language.Translation;
using NLog;
using PQ.HMI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows;

namespace HMI_Master_KX7500.Service.Language
{
    public class LanguageService
    {
        #region PrivateFields

        private static LanguageInitializeModel languageInitializeModel = new LanguageInitializeModel();
        private static Logger logger = LogManager.GetCurrentClassLogger();

        #endregion PrivateFields

        #region Constructor

        public LanguageService()
        {
            initializeMessenger();
        }

        #endregion Constructor

        #region PrivateInitializeMethods

        private void initializeMessenger()
        {
            Messenger.Default.Register<LanguageInitializeModel>(this, initializeLanguage);
        }

        #endregion PrivateInitializeMethods

        #region PrivateMessengerMethods

        private void initializeLanguage(LanguageInitializeModel _languageInitializeModel)
        {
            languageInitializeModel = _languageInitializeModel;
        }

        #endregion PrivateMessengerMethods

        #region PublicMethods

        public static bool SerializeLanguage(List<LanguageModel> language)
        {
            bool value = false;

            try
            {
                if(language.Any(x => string.IsNullOrEmpty(x.LanguageKey)))
                {
                    MessageBox.Show($"{nameof(LanguageModel.LanguageKey)} cannot be null or empty.");
                }
                else if(language.Where(x => x.DefaultLanguage == true).GroupBy(
                    x => x.DefaultLanguage).Any(x => x.Count() > 1))
                {
                    MessageBox.Show($"{nameof(LanguageModel.DefaultLanguage)} repeated.");
                }
                else
                {
                    if(language.Count < 1)
                    {
                        language.Add(new LanguageModel { LanguageKey = "English" });
                        language.Add(new LanguageModel { LanguageKey = "ChineseSimplified" });
                    }

                    if(language.Count > 1 && !language.Any(x => x.DefaultLanguage == true))
                    {
                        language.Find(x => x.LanguageKey == "English").DefaultLanguage = true;
                    }

                    language = language.OrderBy(x => x.LanguageKey).ToList();

                    XmlTools.Serialize(FilePath.Language, language);

                    value = true;

                    Messenger.Default.Send(languageInitializeModel);
                }
            }
            catch(Exception ex) { logger.Error(ex); }

            return value;
        }

        public static List<LanguageModel> DeserializeLanguage()
        {
            List<LanguageModel> language = new List<LanguageModel>();

            if(!File.Exists(FilePath.Language))
            {
                SerializeLanguage(language);
            }

            language = XmlTools.Deserialize<List<LanguageModel>>(FilePath.Language);

            return language;
        }

        public static LanguageModel GetDefaultLanguage(List<LanguageModel> language)
        {
            LanguageModel languageModel = new LanguageModel();

            if(language.Exists(x => x.DefaultLanguage == true))
            {
                languageModel = language.Find(x => x.DefaultLanguage == true);
            }

            return languageModel;
        }

        public static void SerializeLanguageTranslationList(string languageKey,
            List<LanguageTranslationModel> languageTranslationList)
        {
            string filePath = $"{FolderPath.LanguageFolderPath}{languageKey}.xml";

            languageTranslationList = languageTranslationList.OrderBy(x => x.LanguageTranslationKey).ToList();

            XmlTools.Serialize(filePath, languageTranslationList);
        }

        public static List<LanguageTranslationModel> DeserializeLanguageTranslationList(string languageKey)
        {
            List<LanguageTranslationModel> languageTranslationList = new List<LanguageTranslationModel>();

            string filePath = $"{FolderPath.LanguageFolderPath}{languageKey}.xml";

            if(!File.Exists(filePath))
            {
                ResourceDictionary resourceDictionary = new ResourceDictionary
                {
                    Source = new Uri($"pack://application:,,,/HMI_Master_KX7500;component/Languages/{languageKey}.xaml")
                };

                foreach(var item in resourceDictionary.Keys)
                {
                    if(languageTranslationList.Exists(x => x.LanguageTranslationKey == item.ToString()))
                    {
                        languageTranslationList.Find(x => x.LanguageTranslationKey ==
                        item.ToString()).LanguageTranslationValue = resourceDictionary[item].ToString();
                    }
                    else
                    {
                        languageTranslationList.Add(new LanguageTranslationModel
                        {
                            LanguageTranslationKey = item.ToString(),
                            LanguageTranslationValue = resourceDictionary[item].ToString()
                        });
                    }
                }

                //bool defaultLanguageTranslationListExists = false;

                //List<LanguageModel> language = DeserializeLanguage();
                //if (language.Exists(x => x.DefaultLanguage == true))
                //{
                //    LanguageModel languageModel = language.Find(x => x.DefaultLanguage == true);

                //    string _filePath = $"{languageInitializeModel.LanguageFolderPath}" +
                //    $"{languageModel.LanguageKey}{FilePath.XmlExtension}";
                //    LogHelper.KeyValue(nameof(LanguageService), nameof(_filePath), _filePath);

                //    if (File.Exists(_filePath))
                //    {
                //        List<LanguageTranslationModel> _languageTranslationList = DeserializeLanguageTranslationList(
                //            languageModel.LanguageKey);
                //        _languageTranslationList.ForEach(x =>
                //        {
                //            MethodHelper.Action(nameof(LanguageService), () =>
                //            {
                //                if (languageTranslationList.Exists(
                //                    y => y.LanguageTranslationKey == x.LanguageTranslationKey))
                //                {
                //                    languageTranslationList.Find(y => y.LanguageTranslationKey ==
                //                    x.LanguageTranslationKey).LanguageTranslationValue = x.LanguageTranslationValue;
                //                }
                //                else
                //                {
                //                    languageTranslationList.Add(new LanguageTranslationModel
                //                    {
                //                        LanguageTranslationKey = x.LanguageTranslationKey,
                //                        LanguageTranslationValue = x.LanguageTranslationValue
                //                    });
                //                }
                //            });
                //        });

                //        defaultLanguageTranslationListExists = true;
                //    }
                //}

                //if (!defaultLanguageTranslationListExists)
                //{
                //    ResourceDictionary resourceDictionary = new ResourceDictionary
                //    {
                //        Source = new Uri(HmiHelper.GetLanguageSource(HmiHelper.DefaultLanguage))
                //    };

                //    foreach (var item in resourceDictionary.Keys)
                //    {
                //        MethodHelper.Action(nameof(LanguageService), () =>
                //        {
                //            if (languageTranslationList.Exists(x => x.LanguageTranslationKey == item.ToString()))
                //            {
                //                languageTranslationList.Find(x => x.LanguageTranslationKey ==
                //                item.ToString()).LanguageTranslationValue = resourceDictionary[item].ToString();
                //            }
                //            else
                //            {
                //                languageTranslationList.Add(new LanguageTranslationModel
                //                {
                //                    LanguageTranslationKey = item.ToString(),
                //                    LanguageTranslationValue = resourceDictionary[item].ToString()
                //                });
                //            }
                //        });
                //    }
                //}

                if(languageTranslationList.Count == 0)
                {
                    resourceDictionary = new ResourceDictionary
                    {
                        Source = new Uri($"pack://application:,,,/HMI_Master_KX7500;component/Languages/English.xaml")
                    };

                    foreach(var item in resourceDictionary.Keys)
                    {
                        if(languageTranslationList.Exists(x => x.LanguageTranslationKey == item.ToString()))
                        {
                            languageTranslationList.Find(x => x.LanguageTranslationKey ==
                            item.ToString()).LanguageTranslationValue = resourceDictionary[item].ToString();
                        }
                        else
                        {
                            languageTranslationList.Add(new LanguageTranslationModel
                            {
                                LanguageTranslationKey = item.ToString(),
                                LanguageTranslationValue = resourceDictionary[item].ToString()
                            });
                        }
                    }
                }

                SerializeLanguageTranslationList(languageKey, languageTranslationList);
            }

            languageTranslationList = XmlTools.Deserialize<List<LanguageTranslationModel>>(filePath);

            return languageTranslationList;
        }

        public static List<LanguageTranslationEditModel> GetLanguageTranslationEditList()
        {
            List<LanguageTranslationEditModel> languageTranslationEditList = new List<LanguageTranslationEditModel>();

            List<LanguageModel> language = DeserializeLanguage();

            language.ForEach(x =>
            {
                List<LanguageTranslationModel> languageTranslationList = DeserializeLanguageTranslationList(
                        x.LanguageKey);

                languageTranslationList.ForEach(y =>
                {
                    if(languageTranslationEditList.Exists(
                            z => z.LanguageTranslationKey == y.LanguageTranslationKey))
                    {
                        languageTranslationEditList.Find(z => z.LanguageTranslationKey ==
                        y.LanguageTranslationKey).LanguageTranslationValueList.Add(y);
                    }
                    else
                    {
                        languageTranslationEditList.Add(new LanguageTranslationEditModel
                        {
                            LanguageTranslationKey = y.LanguageTranslationKey,
                            LanguageTranslationValueList = new ObservableCollection<LanguageTranslationModel>
                            {
                                y
                            }
                        });
                    }
                });
            });

            return languageTranslationEditList;
        }

        public static bool SaveLanguageTranslationEditList(List<LanguageModel> language,
            List<LanguageTranslationEditModel> languageTranslationEditList)
        {
            bool value = false;

            try
            {
                if(language.Any(x => string.IsNullOrEmpty(x.LanguageKey)))
                {
                    MessageBox.Show($"{nameof(LanguageModel.LanguageKey)} cannot be null or empty.");
                }
                else if(languageTranslationEditList.Any(x => string.IsNullOrEmpty(x.LanguageTranslationKey)))
                {
                    MessageBox.Show($"{nameof(LanguageTranslationModel.LanguageTranslationKey)} cannot be null or empty.");
                }
                else if(languageTranslationEditList.SelectMany(x => x.LanguageTranslationValueList).Any(
                    x => string.IsNullOrEmpty(x.LanguageTranslationValue)))
                {
                    MessageBox.Show($"{nameof(LanguageTranslationModel.LanguageTranslationValue)} cannot be null or empty.");
                }
                else
                {
                    for(int i = 0; i < language.Count; i++)
                    {
                        List<LanguageTranslationModel> languageTranslationList = new List<LanguageTranslationModel>();

                        //languageTranslationEditList.ForEach(x =>
                        //{
                        //    MethodHelper.Action(nameof(LanguageService), () =>
                        //    {
                        //        if (languageTranslationList.Exists(
                        //            y => y.LanguageTranslationKey == x.LanguageTranslationKey))
                        //        {
                        //            languageTranslationList.Find(y => y.LanguageTranslationKey ==
                        //            x.LanguageTranslationKey).LanguageTranslationValue =
                        //            x.LanguageTranslationValueList[i].LanguageTranslationValue;
                        //        }
                        //        else
                        //        {
                        //            languageTranslationList.Add(new LanguageTranslationModel
                        //            {
                        //                LanguageTranslationKey = x.LanguageTranslationKey,
                        //                LanguageTranslationValue = x.LanguageTranslationValueList[i].LanguageTranslationValue
                        //            });
                        //        }
                        //    });
                        //});
                        languageTranslationEditList.Distinct().ToList().ForEach(x =>
                        {
                            languageTranslationList.Add(new LanguageTranslationModel
                            {
                                LanguageTranslationKey = x.LanguageTranslationKey,
                                LanguageTranslationValue = x.LanguageTranslationValueList[i].LanguageTranslationValue
                            });
                        });
                        SerializeLanguageTranslationList(language[i].LanguageKey, languageTranslationList);
                    }

                    value = true;

                    Messenger.Default.Send(languageInitializeModel);
                }
            }
            catch(Exception ex) { logger.Error(ex); }

            return value;
        }

        public static void ChangeLanguage(LanguageModel languageModel)
        {
            Application.Current.Resources.MergedDictionaries.ToList().ForEach(x =>
            {
                if(x.Source == null || x.Source.AbsoluteUri.Contains($"pack://application:,,,/HMI;component/Languages/"))
                {
                    Application.Current.Resources.MergedDictionaries.Remove(x);
                }
            });

            List<LanguageTranslationModel> languageTranslationList = DeserializeLanguageTranslationList(
                languageModel.LanguageKey);

            ResourceDictionary resourceDictionary = new ResourceDictionary();

            languageTranslationList.ForEach(x =>
            {
                if(resourceDictionary.Contains(x.LanguageTranslationKey))
                {
                    resourceDictionary[x.LanguageTranslationKey] = x.LanguageTranslationValue;
                }
                else
                {
                    resourceDictionary.Add(x.LanguageTranslationKey, x.LanguageTranslationValue);
                }
            });

            Application.Current.Resources.MergedDictionaries.Add(resourceDictionary);

            Messenger.Default.Send(languageModel);
        }

        #endregion PublicMethods
    }
}