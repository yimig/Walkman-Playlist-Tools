# Walkman-Playlist-Tools
## 一个制作Walkman播放列表的小应用

 ---
 
 ![主界面](http://upane.cn/wp-content/uploads/2019/02/Walkman_Playlist_Tools_2019-02-09_15-40-56.png)
 
 ### 项目页/下载见：[upane.cn/archives/509](upane.cn/archives/509)
 ### 使用教程见：[http://upane.cn/archives/1140](http://upane.cn/archives/1140)
 
 ---

### 待添加的功能：
1. 智能播放列表（锁定文件夹、其他播放列表）
1. 增量刷新数据 _（已添加）_
1. qq源 **（正在添加）**
1. 文件替换功能
1. 修改导入本地播放列表的逻辑
1. 错误报告
1. 应用启动时，记忆上次关闭的工作区位置

### 大家希望添加的功能（可行性未知)
1. 跳过歌词下载间隔时间 _(暂不可行，会引发网易云/qq音乐的反爬虫机制导致窗口堵塞）_
1. 自动添加专辑图片
1. 单击标题栏排列曲目 _（已添加）_

### bugs：
1. 据说某些情况下保存播放列表会有设备未就绪（IOException）的bug
1. 在SD卡中创建播放列表（单个保存）会保存到机身内存中！  _(已修复，待测试)_
1. 在最新版windows中，按导入时间分类会引起崩溃  _（已修复）_
1. 曲长、年代、同步时间等数字排序不能按照文本格式排 _(已修复)_
1. 未联网时检查更新会崩溃 _(已修复）_
1. 按日期归类音乐不起作用
