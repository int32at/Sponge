use master;
go

create database SpongeDb
go

use SpongeDb;
go

CREATE TABLE Logs ( 
    [id] [uniqueidentifier] ROWGUIDCOL  NOT NULL, 
    [entered_date] [datetime] NULL, 
    [log_date] [varchar](100) NULL, 
    [log_level] [varchar](100) NULL, 
    [log_logger] [varchar](8000) NULL, 
	[log_version] [varchar] (100) NULL,
    [log_message] [varchar](8000) NULL, 
    [log_machine_name] [varchar](8000) NULL, 
    [log_user_name] [varchar](8000) NULL, 
    [log_call_site] [varchar](8000) NULL, 
    [log_thread] [varchar](100) NULL, 
    [log_exception] [varchar](8000) NULL, 
    [log_stacktrace] [varchar](8000) NULL, 
CONSTRAINT [PK_system_logging] PRIMARY KEY CLUSTERED 
( 
    [id] ASC 
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY] 
) ON [PRIMARY]
GO

ALTER TABLE Logs ADD CONSTRAINT [DF_Logs_id]  DEFAULT (newid()) FOR [id] 
GO

ALTER TABLE Logs ADD  CONSTRAINT [DF_Logs_enteredDate]  DEFAULT (getdate()) FOR [entered_date] 
GO

SET ANSI_PADDING OFF 
GO

exec sp_addlogin 'spongeloguser','pass@word1', SpongeDb
go

exec sp_grantdbaccess 'spongeloguser','spongeloguser'
go

grant insert,select on Logs to spongeloguser
go