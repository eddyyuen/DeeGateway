# DeeGateway
基于Bumblebee的一个网关管理系统。
已经用于生产环境连续运行18个月。

## 功能
 - 使用Layui作为后台管理UI
 - 配置文件保存至数据库（Sqlite、MySql、SqlServer等）
 - 更加好用的插件
 
## 安装
 **Sqlite**
 - 使用项目根目录下的 DeeGateway.sqlite
 
 **MySql**
 - 新建数据库，导入 DeeGateway\Sql\deegateway.sql 即可
 
 ## 目录结构
 - DeeGateway 主项目，启动
 - DeeGateway.Cache  缓存
 - DeeGateway.Configuration 后台管理
 - DeeGateway.Plugin  插件
 - DeeGateway.Repository 数据仓库
 - DeeGateway.Utils 工具
 - GenEmbeddedResourceXml Configuration 项目的工具
 - NetCoreRateLimit 限流插件
## 插件介绍
 ![img](/plugin_jwt_list.png)
  ![img](/plugin_jwt_form.png)
