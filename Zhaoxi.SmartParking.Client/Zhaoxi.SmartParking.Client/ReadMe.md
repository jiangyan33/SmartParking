项目分层

DAL数据访问层，各种的DataAccess,比如Web接口，直连数据库，本地Sqlite等等，返回的结果是最原始的数据

BLL业务逻辑层，将DAL返回的数据进行封装处理，适合客户端使用

Prism模块化开发