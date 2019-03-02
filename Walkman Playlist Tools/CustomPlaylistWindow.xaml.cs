using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;

namespace Walkman_Playlist_Tools
{
    /// <summary>
    /// CustomPlaylistWindow.xaml 的交互逻辑
    /// </summary>
    public partial class CustomPlaylistWindow : Window
    {
        private ListView mainListView;
        private ObservableCollection<MusicInfo> resultCollection;

        public ObservableCollection<MusicInfo> ResultCollection => resultCollection;

        private class MatchMusicInfo
        {
            private CustomPlaylistWindow customWindow;

            private bool isMatchTitle;
            private bool isIncludeTitle;
            private bool isRegexTitle;
            private List<string> TitleRule;

            private bool isMatchFormat;
            private bool isIncludeFormat;
            private bool isRegexFormat;
            private List<string> FormatRule;

            private bool isMatchArtist;
            private bool isIncludeArtist;
            private bool isRegexArtist;
            private List<string> ArtistRule;

            private bool isMatchAlbum;
            private bool isIncludeAlbum;
            private bool isRegexAlbum;
            private List<string> AlbumRule;

            private bool isMatchLength;
            private bool isGreaterLength;
            private string LengthRule;

            private bool isMatchYear;
            private bool isGreaterYear;
            private string YearRule;

            private bool isMatchBuildTime;
            private bool isGreaterBuildTime;
            private DateTime BuildTimeRule;

            private bool isMatchPath;
            private bool isIncludePath;
            private bool isRegexPath;
            private List<string> PathRule;

            public MatchMusicInfo(CustomPlaylistWindow customWindow)
            {
                this.customWindow = customWindow;
                LoadEnableRule();
                CheckAllEnable();
                if(isMatchTitle)LoadTitle();
                if(isMatchFormat)LoadFormat();
                if (isMatchArtist)LoadArtist();
                if(isMatchAlbum)LoadAlbum();
                if(isMatchLength)LoadLength();
                if(isMatchYear)LoadYear();
                if(isMatchBuildTime)LoadBuildTime();
                if(isMatchPath) LoadPath();
            }

            private void CheckAllEnable()
            {
                if(!(isMatchTitle||isMatchFormat||isMatchArtist||isMatchAlbum||isMatchLength||isMatchYear||isMatchBuildTime||isMatchPath))throw new NullEnableRuleException();
            }

            private bool ComboBoxChoose(ComboBox box)
            {
                if (box.SelectedIndex == 0) return true;
                else return false;
            }

            private bool CheckedBoxChoose(CheckBox box)
            {
                return (bool) box.IsChecked;
            }

            private List<string> SplitBySemicolon(string text)
            {
                return text.Split(new char[] {';'}).ToList();
            }

            private void LoadEnableRule()
            {
                isMatchTitle = CheckedBoxChoose(customWindow.EnableTitle);
                isMatchFormat = CheckedBoxChoose(customWindow.EnableFormat);
                isMatchArtist = CheckedBoxChoose(customWindow.EnableArtist);
                isMatchAlbum = CheckedBoxChoose(customWindow.EnableAlbum);
                isMatchLength = CheckedBoxChoose(customWindow.EnableLength);
                isMatchYear = CheckedBoxChoose(customWindow.EnableYear);
                isMatchBuildTime = CheckedBoxChoose(customWindow.EnableBuildTime);
                isMatchPath = CheckedBoxChoose(customWindow.EnablePath);
            }

            private void LoadTitle()
            {
                TitleRule = new List<string>();
                isIncludeTitle = ComboBoxChoose(customWindow.IncludeRuleTitle);
                isRegexTitle = CheckedBoxChoose(customWindow.RegexTitile);
                if (isRegexTitle) TitleRule.Add(customWindow.RuleTitle.Text);
                else TitleRule = SplitBySemicolon(customWindow.RuleTitle.Text);
            }

            private void LoadFormat()
            {
                FormatRule=new List<string>();
                isIncludeFormat = ComboBoxChoose(customWindow.IncludeRuleFormat);
                isRegexFormat = CheckedBoxChoose(customWindow.RegexFormat);
                if (isRegexFormat) FormatRule.Add(customWindow.RuleFormat.Text);
                else FormatRule = SplitBySemicolon(customWindow.RuleFormat.Text);
            }

            private void LoadArtist()
            {
                ArtistRule = new List<string>();
                isIncludeArtist = ComboBoxChoose(customWindow.IncludeRuleArtist);
                isRegexArtist = CheckedBoxChoose(customWindow.RegexArtist);
                if (isRegexArtist) ArtistRule.Add(customWindow.RuleArtist.Text);
                else ArtistRule = SplitBySemicolon(customWindow.RuleArtist.Text);
            }

            private void LoadAlbum()
            {
                AlbumRule = new List<string>();
                isIncludeAlbum = ComboBoxChoose(customWindow.IncludeRuleAlbum);
                isRegexAlbum = CheckedBoxChoose(customWindow.RegexAlbum);
                if (isRegexAlbum) AlbumRule.Add(customWindow.RuleAlbum.Text);
                else AlbumRule = SplitBySemicolon(customWindow.RuleAlbum.Text);
            }

            private void LoadLength()
            {
                isGreaterLength = ComboBoxChoose(customWindow.GreaterRuleLength);
                LengthRule = customWindow.RuleLength.Text;
            }

            private void LoadYear()
            {
                isGreaterYear = ComboBoxChoose(customWindow.GreaterRuleYear);
                YearRule = customWindow.RuleYear.Text;
            }

            private void LoadBuildTime()
            {
                isGreaterBuildTime = ComboBoxChoose(customWindow.GreaterRuleBuildTime);
                BuildTimeRule = customWindow.RuleBuildTime.DisplayDate;
            }

            private void LoadPath()
            {
                PathRule = new List<string>();
                isIncludePath = ComboBoxChoose(customWindow.IncludeRulePath);
                isRegexPath = CheckedBoxChoose(customWindow.RegexPath);
                if (isRegexPath) PathRule.Add(customWindow.RulePath.Text);
                else PathRule = SplitBySemicolon(customWindow.RulePath.Text);
            }

            private DateTime ConvertDate(string date)
            {
                var time = date.Split(new char[]{' '})[0].Split(new char[] {'/'});
                return new DateTime(Convert.ToInt32(time[0]),Convert.ToInt32(time[1]),Convert.ToInt32(time[2]));
            }

            private bool MatchText(string text,bool isRegexText,List<string> TextRule,bool isIncludeText)
            {
                if (isRegexText)
                {
                    var regex = new Regex(TextRule[0]);
                    var MatchRegex = regex.IsMatch(text);
                    if (isIncludeText && MatchRegex) return true;
                    else if (!isIncludeText && !MatchRegex) return true;
                    else return false;
                }
                else
                {
                    var MatchContent = false;
                    foreach (var rule in TextRule)
                    {
                        if (text.Contains(rule))
                        {
                            MatchContent = true;
                            break;
                        }
                    }
                    if (isIncludeText && MatchContent) return true;
                    else if (!isIncludeText && !MatchContent) return true;
                    else return false;
                }
            }

            private bool MatchNum(int num, int numRule, bool isGreaterNum)
            {
                if (isGreaterNum && num >= numRule) return true;
                else if (!isGreaterNum && num <= numRule) return true;
                else return false;
            }

            private bool MatchTitle(string title)
            {
                return MatchText(title, isRegexTitle, TitleRule, isIncludeTitle);
            }

            private bool MatchFormat(string format)
            {
                return MatchText(format, isRegexFormat, FormatRule, isIncludeFormat);
            }

            private bool MatchArtist(string artist)
            {
                return MatchText(artist, isRegexArtist, ArtistRule, isIncludeArtist);
            }

            private bool MatchAlbum(string album)
            {
                return MatchText(album, isRegexAlbum, AlbumRule, isIncludeAlbum);
            }

            private bool MatchLength(string length)
            {
                return MatchNum(Convert.ToInt32(length), Convert.ToInt32(LengthRule), isGreaterLength);
            }

            private bool MatchBuildTime(string buildTime)
            {
                var itemTime = ConvertDate(buildTime);
                if (isGreaterBuildTime && itemTime >= BuildTimeRule) return true;
                else if (!isGreaterBuildTime && itemTime <= BuildTimeRule) return true;
                else return false;
            }

            private bool MatchYear(string year)
            {
                try
                {
                    return MatchNum(Convert.ToInt32(year), Convert.ToInt32(YearRule), isGreaterYear);
                }
                catch (Exception)
                {
                    return false;
                }
            }

            private bool MatchPath(string path)
            {
                return MatchText(path, isRegexPath, PathRule, isIncludePath);
            }

            public bool MatchInfo(MusicInfo info)
            {
                var boolList = new List<bool>();
                var result=true;
                if (isMatchTitle) boolList.Add(MatchTitle(info.Title));
                if (isMatchFormat) boolList.Add(MatchFormat(info.Format));
                if(isMatchArtist)boolList.Add(MatchArtist(info.Artist));
                if(isMatchAlbum)boolList.Add(MatchAlbum(info.Album));
                if(isMatchLength)boolList.Add(MatchLength(info.Length.ToString()));
                if(isMatchYear)boolList.Add(MatchYear(info.Year));
                if(isMatchBuildTime)boolList.Add(MatchBuildTime(info.Buildtime));
                if(isMatchPath)boolList.Add(MatchPath(info.Path));
                foreach (var value in boolList)
                {
                    result = result && value;
                }

                return result;
            }
        }

        public CustomPlaylistWindow(ListView mainListView)
        {
            this.mainListView = mainListView;
            InitializeComponent();
        }

        private void Expander_OnExpanded(object sender, RoutedEventArgs e)
        {
            (sender as Expander).Height = 630;
            Height = 830;
        }

        private void Expander_OnCollapsed(object sender, RoutedEventArgs e)
        {
            (sender as Expander).Height = 23;
            Height = 225;
        }

        private void GenerateButton_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                var matchInfo = new MatchMusicInfo(this);
                resultCollection = new ObservableCollection<MusicInfo>();
                foreach (MusicInfo info in mainListView.Items)
                {
                    if (matchInfo.MatchInfo(info)) resultCollection.Add(info);
                }
                Close();
            }
            catch (NullEnableRuleException) { }
            catch (Exception) { }
        }
    }

    public class NullEnableRuleException : Exception
    {

    }
}
