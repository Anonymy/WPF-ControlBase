﻿using HeBianGu.Applications.ControlBase.LinkWindow;
using HeBianGu.Base.WpfBase;
using HeBianGu.Control.Message;
using HeBianGu.Control.MessageContainer;
using HeBianGu.General.WpfControlLib;
using HeBianGu.Service.Validation;
using HeBianGu.Systems.Upgrade;
using HeBianGu.Window.MessageDialog;
using HeBianGu.Window.Notify;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using System.Windows.Shell;

namespace HeBianGu.App.Blur
{
    internal class ShellViewModel : NotifyPropertyChanged
    {

        private string _buttonContentText;
        /// <summary> 说明  </summary>
        public string ButtonContentText
        {
            get { return _buttonContentText; }
            set
            {
                _buttonContentText = value;
                RaisePropertyChanged("ButtonContentText");
            }
        }


        private ObservableSource<TreeNodeEntity> _observableSource = new ObservableSource<TreeNodeEntity>();
        /// <summary> 说明  </summary>
        public ObservableSource<TreeNodeEntity> ObservableSource
        {
            get { return _observableSource; }
            set
            {
                _observableSource = value;
                RaisePropertyChanged("ObservableSource");
            }
        }

        private ObservableCollection<MessageBase> _messageSource = new ObservableCollection<MessageBase>();
        /// <summary> 说明  </summary>
        public ObservableCollection<MessageBase> MessageSource
        {
            get { return _messageSource; }
            set
            {
                _messageSource = value;
                RaisePropertyChanged("MessageSource");
            }
        }


        private StoryBoardPlayerViewModel _storyBoardPlayerViewModel = new StoryBoardPlayerViewModel();
        /// <summary> 说明  </summary>
        public StoryBoardPlayerViewModel StoryBoardPlayerViewModel
        {
            get { return _storyBoardPlayerViewModel; }
            set
            {
                _storyBoardPlayerViewModel = value;
                RaisePropertyChanged("StoryBoardPlayerViewModel");
            }
        }

        private ObservableCollection<TreeNodeEntity> _collection = new ObservableCollection<TreeNodeEntity>();
        /// <summary> 说明  </summary>
        public ObservableCollection<TreeNodeEntity> Collection
        {
            get { return _collection; }
            set
            {
                _collection = value;
                RaisePropertyChanged("Collection");
            }
        }

        private List<TreeNodeEntityViewModel> _nodes = new List<TreeNodeEntityViewModel>();
        /// <summary> 说明  </summary>
        public List<TreeNodeEntityViewModel> Nodes
        {
            get { return _nodes; }
            set
            {
                _nodes = value;
                RaisePropertyChanged("Nodes");
            }
        }

        protected override void Init()
        {

            ObservableSource.PageCount = 6;

            ObservableSource.Clear();

            List<TreeNodeEntity> data = AssemblyDomain.Instance.GetTreeListData();

            ObservableSource.Add(data.ToArray());


            Collection.Clear();

            foreach (TreeNodeEntity item in data)
            {
                Collection.Add(item);
            }

            data = data.Take(200).ToList();

            List<TreeNodeEntityViewModel> collection = new List<TreeNodeEntityViewModel>();

            foreach (TreeNodeEntity item in data)
            {
                collection.Add(new TreeNodeEntityViewModel(item) { IsExpanded = true });
            }

            Nodes = Bind(collection);
        }

        /// <summary>
        /// 绑定树
        /// </summary>
        private List<TreeNodeEntityViewModel> Bind(List<TreeNodeEntityViewModel> nodes)
        {
            List<TreeNodeEntityViewModel> outputList = new List<TreeNodeEntityViewModel>();

            for (int i = 0; i < nodes.Count; i++)
            {
                if (string.IsNullOrEmpty(nodes[i].ParentID))
                {
                    outputList.Add(nodes[i]);
                }
                else
                {
                    TreeNodeEntityViewModel result = FindDownward(nodes, nodes[i].ParentID);

                    if (result != null)
                    {
                        nodes[i].Parent = result;

                        result.Nodes.Add(nodes[i]);
                    }
                }
            }
            return outputList;
        }

        /// <summary>
        /// 递归向下查找
        /// </summary>
        private TreeNodeEntityViewModel FindDownward(List<TreeNodeEntityViewModel> nodes, string id)
        {
            if (nodes == null) return null;

            for (int i = 0; i < nodes.Count; i++)
            {
                if (nodes[i].ID == id)
                {
                    return nodes[i];
                }
                TreeNodeEntityViewModel node = FindDownward(nodes[i].Nodes, id);

                if (node != null)
                {
                    return node;
                }
            }
            return null;
        }

        public ObservableCollection<SearchComboBoxItemModel> ComboBoxItems
        {
            get { return _comboBoxItems; }
            set { _comboBoxItems = value; RaisePropertyChanged("ComboBoxItems"); }
        }
        private ObservableCollection<SearchComboBoxItemModel> _comboBoxItems = new ObservableCollection<SearchComboBoxItemModel>();
        private Random random = new Random();

        /// <summary> 命令通用方法 </summary>
        protected override async void RelayMethod(object obj)

        {
            string command = obj?.ToString();

            //  Do：对话消息
            if (command == "Button.ShowDialogMessage")
            {
                await Message.Instance.ShowSumitMessge("这是消息对话框？");
            }

            //  Do：等待消息
            else if (command == "Button.ShowWaittingMessge")
            {
                await Message.Instance.ShowWaittingMessge(() => Thread.Sleep(2000));
            }

            //  Do：百分比进度对话框
            else if (command == "Button.ShowPercentProgress")
            {
                Action<IPercentProgress> action = l =>
                    {
                        for (int i = 0; i < 100; i++)
                        {
                            l.Value = i;

                            Thread.Sleep(10);
                        }

                        Thread.Sleep(1000);

                        Message.Instance.ShowSnackMessageWithNotice("加载完成！");
                    };
                await Message.Instance.ShowPercentProgress(action);

            }

            //  Do：文本进度对话框
            else if (command == "Button.ShowStringProgress")
            {
                Action<IStringProgress> action = l =>
                {
                    for (int i = 1; i <= 100; i++)
                    {
                        l.MessageStr = $"正在提交当前页第{i}份数据,共100份";

                        Thread.Sleep(10);
                    }

                    Thread.Sleep(1000);

                    Message.Instance.ShowSnackMessageWithNotice("提交完成：成功100条，失败0条！");
                };

                await Message.Instance.ShowStringProgress(action);

            }
            //  Do：文本进度对话框
            else if (command == "Button.ShowTakStringProgress")
            {
                //Action<IStringProgress> action = l =>
                //{
                //    for (int i = 1; i <= 100; i++)
                //    {
                //        l.MessageStr = $"正在提交当前页第{i}份数据,共100份";

                //        Thread.Sleep(10);
                //    }

                //    Thread.Sleep(1000);

                //    Message.Instance.ShowSnackMessageWithNotice("提交完成：成功100条，失败0条！");
                //};

                StringProgressDialog dialog = new StringProgressDialog();

                Message.Instance.ShowLayer(dialog);

            }


            //  Do：确认取消对话框
            else if (command == "Button.ShowResultMessge")
            {
                bool result = await Message.Instance.ShowResultMessge("确认要退出系统?");

                if (result)
                {
                    Message.Instance.ShowSnackMessageWithNotice("你点击了取消");
                }
                else
                {
                    Message.Instance.ShowSnackMessageWithNotice("你点击了确定");
                }
            }

            //  Do：提示消息
            else if (command == "Button.ShowSnackMessage")
            {

                //ErrorMessage error = new ErrorMessage() { Message= "这是提示消息？" };

                //MessageService.ShowSnackMessage(error);

                Message.Instance.ShowSnackMessageWithNotice("这是提示消息？");

            }

            //  Do：气泡消息
            else if (command == "Button.ShowNotifyMessage")
            {
                Message.Instance.ShowNotifyMessage("你有一条报警信息需要处理，请检查", "Notify By HeBianGu");
            }

            //  Do：气泡消息
            else if (command == "Button.ShowIdentifyNotifyMessage")
            {
                Notify.Instance.ShowNotifyDialogMessage("自定义气泡消息" + DateTime.Now.ToString("yyyy-mm-dd HH:mm:ss"), "友情提示", 5);
            }

            //  Do：气泡消息
            else if (command == "Button.ShowWindowSumitMessage")
            {
                MessageDialogWindow.ShowSumit("这是窗口提示消息", "提示", true);
            }

            //  Do：气泡消息
            else if (command == "Button.ShowWindowResultMessage")
            {
                MessageDialogWindow.ShowDialog("这是窗口提示消息", "提示", -1, true);
            }

            //  Do：气泡消息
            else if (command == "Button.ShowWindowIndentifyMessage")
            {

                List<Tuple<string, Action<IMessageDialogWindow>>> acts = new List<Tuple<string, Action<IMessageDialogWindow>>>();


                Action<IMessageDialogWindow> action = l =>
                  {
                      l.CloseAnimation(l as WindowBase);

                      l.Result = true;

                      Message.Instance.ShowSnackMessageWithNotice("你点到我了！");
                  };

                acts.Add(Tuple.Create("按钮一", action));
                acts.Add(Tuple.Create("按钮二", action));
                acts.Add(Tuple.Create("按钮三", action));

                MessageDialogWindow.ShowDialogWith("这是自定义按钮提示消息", "好心提醒", true, acts.ToArray());
            }

            //  Do：气泡消息
            else if (command == "Button.Upgrade")
            {

                UpgradeWindow window = new UpgradeWindow
                {
                    TitleMessage = "发现新版本：V3.0.1"
                };
                List<string> message = new List<string>
                {
                    "1、增加了检验更新和版本下载",
                    "2、增加了Mvc跳转页面方案",
                    "3、修改了一些已知BUG"
                };
                window.Message = message;

                bool? find = window.ShowDialog();

                if (find.HasValue && find.Value)
                {
                    DownLoadWindow downLoad = new DownLoadWindow
                    {
                        TitleMessage = "正在下载新版本：V3.0.1",
                        Url = @"http://download.blender.org/peach/bigbuckbunny_movies/BigBuckBunny_320x180.mp4",
                        Message = message
                    };
                    downLoad.ShowDialog();
                }

                //UpgradeWindow.BeginUpgrade("发现新版本：V3.0.1", @"http://download.blender.org/peach/bigbuckbunny_movies/BigBuckBunny_320x180.mp4",
                //   message.ToArray());
            }

            //  Do：气泡消息
            else if (command.StartsWith("Button.Message.Error"))
            {
                ErrorMessage message = new ErrorMessage
                {
                    Message = "错误信息！"
                };

                AddMessage(message, command);
            }

            //  Do：气泡消息
            else if (command.StartsWith("Button.Message.Info"))
            {
                InfoMessage message = new InfoMessage
                {
                    Message = "提示信息！"
                };

                AddMessage(message, command);

            }

            //  Do：气泡消息
            else if (command.StartsWith("Button.Message.Success"))
            {
                SuccessMessage message = new SuccessMessage
                {
                    Message = "保存成功！"
                };

                AddMessage(message, command);

            }

            //  Do：气泡消息
            else if (command.StartsWith("Button.Message.Fatal"))
            {
                FatalMessage message = new FatalMessage
                {
                    Message = "问题很严重！"
                };

                AddMessage(message, command);


            }

            //  Do：气泡消息
            else if (command.StartsWith("Button.Message.Warn"))
            {
                WarnMessage message = new WarnMessage
                {
                    Message = "警告信息！"
                };

                AddMessage(message, command);

            }

            //  Do：气泡消息
            else if (command.StartsWith("Button.Message.Waitting"))
            {
                Func<WaittingMessage, bool> func = l =>
                   {
                       l.Message = $"正在加载...";

                       Thread.Sleep(5000);

                       return true;
                   };

                bool result = await Notify.Instance.ShowWinWaittingMessage(func);

                if (result)
                {
                    Message.Instance.ShowSnackMessageWithNotice("加载完成");
                }
            }

            //  Do：气泡消息
            else if (command.StartsWith("Button.Message.StringProgrss"))
            {
                Func<StringProgressMessage, bool> func = l =>
                {
                    for (int i = 1; i <= 100; i++)
                    {
                        System.Windows.Application.Current.Dispatcher.Invoke(() =>
                        {
                            l.Message = $"正在提交当前页第{i}份数据,共100份";
                        });


                        Thread.Sleep(50);
                    }

                    return true;
                };

                bool result = await Notify.Instance.ShowWinProgressStrMessage(func);

                if (result)
                {
                    Message.Instance.ShowSnackMessageWithNotice("运行成功");
                }
            }

            //  Do：气泡消息
            else if (command.StartsWith("Button.Message.PercentProgrss"))
            {
                Func<PercentProgressMessage, bool> func = l =>
                {
                    for (int i = 1; i <= 100; i++)
                    {
                        System.Windows.Application.Current.Dispatcher.Invoke(() =>
                        {
                            l.Value = i;
                        });

                        Thread.Sleep(50);
                    }

                    return true;
                };

                bool result = await Notify.Instance.ShowWinProgressBarMessage(func);

                if (result)
                {
                    Message.Instance.ShowSnackMessageWithNotice("运行成功");
                }
            }

            //  Do：气泡消息
            else if (command.StartsWith("Button.Message.Dailog"))
            {
                DailogMessage message = new DailogMessage
                {
                    Message = "可以保存了么?",

                    IsMatch = l =>
                      {
                          if (random.Next(1, 3) == 1)
                          {
                              Message.Instance.ShowSnackMessageWithNotice("保存成功");
                              return true;
                          }
                          else
                          {
                              Message.Instance.ShowSnackMessageWithNotice("保存失败");
                              return false;

                          }
                      }
                };

                AddMessage(message, command);
            }

            //  Do：气泡消息
            else if (command.StartsWith("Button.ShowCoverMessge"))
            {
                SettingControl setting = new SettingControl();

                Message.Instance.ShowLayer(setting);

            }

            //  Do：气泡消息
            else if (command.StartsWith("Button.ShowObjectWithPropertyForm"))
            {
                Student student = new Student();

                //await Message.Instance.ShowObjectWithPropertyForm(student, l => true, "修改学生信息");

            }

            //  Do：气泡消息
            else if (command == "Button.ShowObjectWithContent")
            {
                Student student = new Student();

                Predicate<Student> match = l =>
                     {
                         if (random.Next(3) == 1)
                         {
                             Message.Instance.ShowSnackMessageWithNotice("随机测试提交失败，请再试几次");
                             return false;
                         }
                         else
                         {
                             Message.Instance.ShowSnackMessageWithNotice("随机测试提交成功，请再试几次");
                             return true;
                         }
                     };

                await Message.Instance.ShowObjectWithContent(student, match, "修改学生信息", null, ObjectContentDialog.DefaultKey);

            }

            //  Do：气泡消息
            else if (command == "Button.ShowObjectWithContent.WithValidation")
            {
                StudentViewModel student = new StudentViewModel();

                Predicate<StudentViewModel> match = l =>
                {
                    //if (ObjectPropertyFactory.ModelState(student.Model,out List<string> errors))
                    if (student.ModelState(out List<string> errors))
                    {
                        Message.Instance.ShowSnackMessageWithNotice("提交成功");
                        return true;
                    }
                    else
                    {
                        Message.Instance.ShowSnackMessageWithNotice(errors?.FirstOrDefault());
                        return false;
                    }
                };

                await Message.Instance.ShowObjectWithContent(student, match, "修改学生信息");

            }
            else if (command == "Button.Add")
            {

                if (StoryBoardPlayerViewModel.PlayMode)
                {
                    Message.Instance.ShowSnackMessageWithNotice("请先停止播放再进行添加！");
                    return;
                }
                StoryBoardPlayerViewModel.Create();
            }
            else if (command == "init")
            {
                for (int i = 0; i < 60; i++)
                {
                    ComboBoxItems.Add(new SearchComboBoxItemModel()
                    {
                        Header = "ComboBoxItem" + (ComboBoxItems.Count + 1),
                        Value = (ComboBoxItems.Count + 1),
                        CanDelete = true
                    });
                }
            }

            //  Do：任务栏消息
            else if (command == "Button.Taskbar.Error")
            {
                Message.Instance.ShowTaskbar(l => l.ProgressState = TaskbarItemProgressState.Error);
            }

            //  Do：任务栏消息
            else if (command == "Button.Taskbar.Success")
            {
                Message.Instance.ShowTaskbar(l => l.ProgressState = TaskbarItemProgressState.Normal);
            }

            //  Do：任务栏消息
            else if (command == "Button.Taskbar.Warn")
            {
                Message.Instance.ShowTaskbar(l => l.ProgressState = TaskbarItemProgressState.Paused);
            }

            //  Do：任务栏消息
            else if (command == "Button.Taskbar.Percent")
            {
                await Message.Instance.ShowTaskbarPercent(l =>
                {
                    for (int i = 0; i < 100; i++)
                    {
                        System.Windows.Application.Current.Dispatcher.Invoke(() =>
                        {
                            l.ProgressValue = i / 100.0;
                        });

                        Thread.Sleep(50);
                    }

                });

            }

            //  Do：任务栏消息
            else if (command == "Button.Taskbar.Waitting")
            {
                await Message.Instance.ShowTaskbarWaitting(() =>
                {
                    Thread.Sleep(5000);

                    return true;
                });

            }
            //  Do：任务栏消息
            else if (command == "Button.Taskbar.Image")
            {
                Drawing drawing = System.Windows.Application.Current.FindResource("S.Drawing.Database") as Drawing;

                Message.Instance.ShowTaskbarImage(new DrawingImage(drawing));

            }




        }

        private void AddMessage(MessageBase message, string command)
        {
            if (command.EndsWith("System"))
            {
                Notify.Instance.ShowSysMessage(message);
            }
            else if (command.EndsWith("Window"))
            {

                Notify.Instance.ShowWinMessage(message);
            }
            else
            {
                MessageSource.Add(message);
            }
        }


        private TextBoxViewModel _textBoxViewModel = new TextBoxViewModel();
        /// <summary> 说明  </summary>
        public TextBoxViewModel TextBoxViewModel
        {
            get { return _textBoxViewModel; }
            set
            {
                _textBoxViewModel = value;
                RaisePropertyChanged("TextBoxViewModel");
            }
        }



    }


    public partial class StoryBoardPlayerViewModel : NotifyPropertyChanged
    {
        public StoryBoardPlayerViewModel()
        {
            IndexChanged += l =>
            {
                //  Do ：触发子项进度
                foreach (StoryBoardItemViewModel item in Collection)
                {
                    item.OnIndexChanged(l);
                }
            };

            for (int i = 0; i < 5; i++)
            {
                Create();
            }
        }
        private ObservableCollection<StoryBoardItemViewModel> _collection = new ObservableCollection<StoryBoardItemViewModel>();
        /// <summary> 说明  </summary>
        public ObservableCollection<StoryBoardItemViewModel> Collection
        {
            get { return _collection; }
            set
            {
                _collection = value;
                RaisePropertyChanged("Collection");
            }
        }

        public StoryBoardItemViewModel Create()
        {
            StoryBoardItemViewModel item = new StoryBoardItemViewModel(this);

            //  Do ：注册子项进度
            item.IndexChanged += l =>
            {
                //  Do ：触发子项进度外部事件
                ItemIndexChanged?.Invoke(item, l);
            };

            Collection.Add(item);

            return item;
        }


        private double _maxValue = 100.0;
        /// <summary> 说明  </summary>
        public double MaxValue
        {
            get { return _maxValue; }
            set
            {
                _maxValue = value;
                RaisePropertyChanged("MaxValue");
            }
        }


        private double _minValue = 0.0;
        /// <summary> 说明  </summary>
        public double MinValue
        {
            get { return _minValue; }
            set
            {
                _minValue = value;
                RaisePropertyChanged("MinValue");
            }
        }


        private double _value = 0.0;
        /// <summary> 说明  </summary>
        public double Value
        {
            get { return _value; }
            set
            {
                _value = value;
                RaisePropertyChanged("Value");
            }
        }


        private int _speed = 0;
        /// <summary> 说明  </summary>
        public int Speed
        {
            get { return _speed; }
            set
            {
                _speed = value;
                RaisePropertyChanged("Speed");
            }
        }

        private System.Timers.Timer timer = new System.Timers.Timer();

        public void Start()
        {
            Stop();

            timer = new System.Timers.Timer();

            //timer.Interval = 1000 - Speed;

            timer.Elapsed += (l, k) =>
            {
                timer.Interval = Speed == 0 ? 1000 : 1010 - 125 * Speed;

                if (Value < MaxValue)
                {
                    Value++;
                }
                else
                {
                    Value = 0;
                }

                IndexChanged?.Invoke(Value);
            };

            timer.Start();
        }

        public void StartWith(StoryBoardItemViewModel item)
        {
            Stop();

            Collection.Foreach(l =>
            {
                if (l != item)
                    l.PlayMode = false;

                l.IsEnbled = l != item;
            });


            timer = new System.Timers.Timer();

            Value = MaxValue * item.LeftPercent;

            timer.Interval = Speed == 0 ? 1000 : 1010 - 125 * Speed;

            timer.Elapsed += (l, k) =>
            {
                timer.Interval = Speed == 0 ? 1000 : 1010 - 125 * Speed;



                if (Value < (int)MaxValue * item.RightPercent && Value >= MaxValue * item.LeftPercent)
                {
                    Value++;
                }
                else
                {
                    Value = MaxValue * item.LeftPercent;
                }


                IndexChanged?.Invoke(Value);
            };

            timer.Start();


        }

        public void Stop()
        {
            timer.Stop();
        }


        private bool _playMode;
        /// <summary> 说明  </summary>
        public bool PlayMode
        {
            get { return _playMode; }
            set
            {
                _playMode = value;
                RaisePropertyChanged("PlayMode");
            }
        }




        protected override void RelayMethod(object obj)
        {
            string command = obj.ToString();

            //  Do：应用
            if (command == "Sumit")
            {


            }
            //  Do：取消
            else if (command == "Cancel")
            {


            }
            else if (command == "ToggleButton.Click.Play")
            {
                if (Collection.Count == 0)
                {
                    PlayMode = false;

                    Message.Instance.ShowSnackMessageWithNotice("请至少添加一个条目！");

                    return;
                }

                Collection.Foreach(l => l.PlayMode = true);

                Collection.Foreach(l => l.IsEnbled = false);

                Start();


            }

            else if (command == "ToggleButton.Click.Stop")
            {
                Collection.Foreach(l => l.PlayMode = false);

                Collection.Foreach(l => l.IsEnbled = true);

                Stop();
            }
        }

        public event Action<double> IndexChanged;

        public event Action<StoryBoardItemViewModel, double> ItemIndexChanged;

    }

    public partial class StoryBoardItemViewModel : NotifyPropertyChanged
    {
        private static int Index = 0;

        private string _name;
        /// <summary> 说明  </summary>
        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                RaisePropertyChanged("Name");
            }
        }

        public StoryBoardItemViewModel()
        {

        }

        private StoryBoardPlayerViewModel _parent;

        public StoryBoardItemViewModel(StoryBoardPlayerViewModel parent)
        {
            _parent = parent;

            Name = (Index++).ToString();
        }

        public event Action<double> IndexChanged;

        private bool _isEnbled = false;
        /// <summary> 说明  </summary>
        public bool IsEnbled
        {
            get { return _isEnbled; }
            set
            {
                _isEnbled = value;
                RaisePropertyChanged("IsEnbled");
            }
        }


        private double _leftPercent = 0.0;
        /// <summary> 说明  </summary>
        public double LeftPercent
        {
            get { return _leftPercent; }
            set
            {
                _leftPercent = value;
                RaisePropertyChanged("LeftPercent");
            }
        }


        private double _rightPercent = 1.0;
        /// <summary> 说明  </summary>
        public double RightPercent
        {
            get { return _rightPercent; }
            set
            {
                _rightPercent = value;
                RaisePropertyChanged("RightPercent");
            }
        }

        private bool _playMode;
        /// <summary> 说明  </summary>
        public bool PlayMode
        {
            get { return _playMode; }
            set
            {
                _playMode = value;

                RaisePropertyChanged("PlayMode");
            }
        }


        private double _value = 0;
        /// <summary> 说明  </summary>
        public double Value
        {
            get { return _value; }
            set
            {
                _value = value;
                RaisePropertyChanged("Value");
            }
        }


        public void OnIndexChanged(double value)
        {
            if (IsEnbled == false && PlayMode)
            {
                IndexChanged?.Invoke(value);

                Value = value;
            }
        }

        public RelayCommand<ToggleButton> ToggleButtonCheckChangedCommand => new Lazy<RelayCommand<ToggleButton>>(() => new RelayCommand<ToggleButton>(l => ToggleButtonCheckedChanged(l))).Value;

        internal void ToggleButtonCheckedChanged(ToggleButton commandParamer)
        {
            //  Do ：避免通过属性修改触发此事件
            if (!commandParamer.IsFocused) return;

            if (PlayMode)
            {
                _parent.StartWith(this);
            }
            else
            {
                _parent.Stop();
            }
        }


        protected override async void RelayMethod(object obj)
        {
            string command = obj.ToString();

            //  Do：应用
            if (command == "StoryBoardItem.Button.Delelte")
            {
                if (_parent.PlayMode)
                {
                    Message.Instance.ShowSnackMessageWithNotice("请先停止播放再进行此操作！");
                    return;
                }

                bool result = await Message.Instance.ShowResultMessge("确定要删除当前项目？");

                if (!result) return;

                _parent.Collection.Remove(this);

                IndexChanged = null;
            }
        }
    }


    internal class TextBoxViewModel : ValidationPropertyChanged
    {
        private string _name;
        /// <summary> 说明  </summary>
        [Required(ErrorMessage = "数据不能为空")]
        [RegularExpression(@"^[\u4e00-\u9fa5]{0,}$", ErrorMessage = "只能输入汉字！")]
        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;

                RaisePropertyChanged("Name");
            }
        }


        private string _age;
        /// <summary> 说明  </summary>
        [Required(ErrorMessage = "数据不能为空")]
        [RegularExpression(@"^[0-9]*$", ErrorMessage = "只能输入数字")]
        public string Age
        {
            get { return _age; }
            set
            {
                _age = value;
                RaisePropertyChanged("Age");
            }
        }


        private string _email;
        /// <summary> 说明  </summary>
        [Required(ErrorMessage = "数据不能为空")]
        [RegularExpression(@"^\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$", ErrorMessage = "邮箱地址不合法！")]
        public string Email
        {
            get { return _email; }
            set
            {
                _email = value;
                RaisePropertyChanged("Email");
            }
        }


        private string _phone;
        /// <summary> 说明  </summary>
        [Required(ErrorMessage = "数据不能为空")]
        [RegularExpression(@"^1[3|4|5|7|8][0-9]{9}$", ErrorMessage = "手机号码不合法！")]
        public string Phone
        {
            get { return _phone; }
            set
            {
                _phone = value;
                RaisePropertyChanged("Phone");
            }
        }


        private string _account;
        /// <summary> 说明  </summary>
        [Required(ErrorMessage = "数据不能为空")]
        [RegularExpression(@"^[a-zA-Z][a-zA-Z0-9_]{4,15}$", ErrorMessage = "字母开头，允许5-16字节，允许字母数字下划线！")]
        public string Accont
        {
            get { return _account; }
            set
            {
                _account = value;
                RaisePropertyChanged("Accont");
            }
        }


        private string _passWord;
        /// <summary> 说明  </summary>
        [Required(ErrorMessage = "数据不能为空")]
        [RegularExpression(@"^[a-zA-Z]\w{5,17}$", ErrorMessage = "以字母开头，长度在6~18之间，只能包含字母、数字和下划线！！")]
        public string PassWord
        {
            get { return _passWord; }
            set
            {
                _passWord = value;
                RaisePropertyChanged("PassWord");
            }
        }


        private string _regin;
        /// <summary> 说明  </summary>
        [Required(ErrorMessage = "数据不能为空")]
        [RegularExpression(@"^\d{5}$", ErrorMessage = "只能5位的数字")]
        public string Regin
        {
            get { return _regin; }
            set
            {
                _regin = value;
                RaisePropertyChanged("Regin");
            }
        }


        private string _limit;
        /// <summary> 说明  </summary>
        [Required(ErrorMessage = "数据不能为空")]
        [RegularExpression(@"^\d{5,8}$", ErrorMessage = "只能5-8位的数字")]
        public string Limit
        {
            get { return _limit; }
            set
            {
                _limit = value;
                RaisePropertyChanged("Limit");
            }
        }


        private string _cardID;
        /// <summary> 说明  </summary>
        [Required(ErrorMessage = "数据不能为空")]
        [RegularExpression(@"^\d{15}|\d{18}$", ErrorMessage = "身份证号码不合法！")]
        public string CardID
        {
            get { return _cardID; }
            set
            {
                _cardID = value;
                RaisePropertyChanged("CardID");
            }
        }

        #region - 方法 -

        protected override void RelayMethod(object obj)
        {
            string command = obj.ToString();

            //  Do：应用
            if (command == "Button.Click.CheckDataSumit")
            {
                if (IsValid())
                {
                    Message.Instance.ShowSnackMessageWithNotice("数据校验成功！");
                }
                else
                {
                    Message.Instance.ShowSnackMessageWithNotice("数据校验错误 - " + Error);
                }

            }
            //  Do：取消
            else if (command == "Cancel")
            {


            }
        }

        #endregion

    }

    public partial class TreeNodeEntityViewModel : NotifyPropertyChanged
    {
        public TreeNodeEntityViewModel(TreeNodeEntity entity)
        {
            TreeNodeEntity = entity;

            ID = entity.ID;

            ParentID = entity.ParentID;

            Code = entity.Code;

            Name = entity.Name;

            NamePY = entity.NamePY;

            RootCode = entity.RootCode;
        }

        public TreeNodeEntity TreeNodeEntity { get; set; }

        public string ID { get; set; }

        public string Name { get; set; }

        public string NamePY { get; set; }

        public string Code { get; set; }

        public string RootCode { get; set; }

        public string ParentID { get; set; }


        private bool _isSelected = false;
        /// <summary> 是否选中  </summary>
        public bool IsSelected
        {
            get { return _isSelected; }
            set
            {
                _isSelected = value;
                RaisePropertyChanged("IsSelected");
            }
        }


        private Visibility _visibility;
        /// <summary> 说明  </summary>
        public Visibility Visibility
        {
            get { return _visibility; }
            set
            {
                _visibility = value;
                RaisePropertyChanged("Visibility");
            }
        }



        private bool _isExpanded;
        /// <summary> 是否展开  </summary>
        public bool IsExpanded
        {
            get { return _isExpanded; }
            set
            {
                _isExpanded = value;
                RaisePropertyChanged("IsExpanded");
            }
        }

        public TreeNodeEntityViewModel Parent { get; set; }

        public List<TreeNodeEntityViewModel> Nodes { get; set; } = new List<TreeNodeEntityViewModel>();

    }

    public class TreeNodeEntity
    {
        [Display(Name = "编号")]
        [Required]
        public string Code { get; set; }

        [Display(Name = "跟节点")]
        [Required]
        public string RootCode { get; set; }

        [Display(Name = "拼音")]
        [Required]
        public string NamePY { get; set; }

        [Display(Name = "名称")]
        [Required]
        public string Name { get; set; }

        [Display(Name = "父节点ID")]
        [Required]
        public string ParentID { get; set; }

        [Display(Name = "当前ID")]
        [Required]
        public string ID { get; set; }
    }


}
