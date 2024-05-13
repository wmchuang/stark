
## 迁移
 ##### 脚本都要在对应模块下执行
 
- 创建迁移

` dotnet ef migrations add init1 -s ..\..\Stark.Admin\    `

- 应用迁移

`` dotnet ef database update -s ..\..\Stark.Admin\ ``

- 获取迁移脚本

 ` dotnet ef migrations script -s ..\..\Stark.Admin\ `



PreConfigureServices 添加依赖注入或者其它配置之前
ConfigureServices 添加依赖注入或者其它配置
PostConfigureServices 添加依赖注入或者其它配置之后
OnPreApplicationInitialization 初始化所有模块之前
OnApplicationInitialization 初始化所有模块
OnPostApplicationInitialization 初始化所有模块之后
OnApplicationShutdown 应用关闭执行



add-migration 这个命令一般都不会有啥问题输入命令回车在输入迁移名称就ok

remove-migration 这个也是如果想删除最后一次迁移 直接执行就好

update-database 第一步添加一个迁移文件成功后，可以用该命令直接更新到数据库，默认是所有迁移，如果想指定迁移直接加上迁移文件的名字就好了，如：update-database migrationName,也相当于版本回滚操，比如有版本1，2，3 此时我想回滚到版本1 就直接 update-database 1,此时数据库中已经更新到1版本了，然后在两次remove-migration把2和3的迁移文件删除就好了

Script-Migration 这个命令用于生成迁移文件对应的sql语句的，跟之前的ef貌似有些不一样，该命令如果不加任何参数 是默认生成所有迁移文件对应的sql语句，当然也参照格式指定餐宿

Script-Migration -From migrationName1 -To migrationName2 -Context ContextName
有意思的是 ，它不会生成from对应的迁移文件的sql，也就是说想上面这么写只会生生成migrationName2的sql语句，那么问题来了 ，需要生成第一个迁移文件的sql怎么办？经过查看官方文档，需要指定from参数为0，也就是 Script-Migration -From 0


dotnet ef migrations add 生成一条迁移

dotnet ef migrations remove 删除最新一次迁移

dotnet ef database update 生成迁移到数据库，跟上面pmc命令类似 后面加指定的迁移作为参数可以进行版本的回滚

dotnet ef migrations script 也跟pmc类似 如果没有任何参数的话默认是生成所有sql脚本，但是参数格式略有不同如下：dotnet ef migrations script migrationName1 migrationName2 ; 是像这样直接跟迁移名称的也就是生成migrationName1 到migrationName2 的sql脚本