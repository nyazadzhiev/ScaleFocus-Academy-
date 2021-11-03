CREATE TABLE [dbo].[Users](
		[Id] [int] IDENTITY(1,1) NOT NULL,
		[FirstName] [NVARCHAR](50) NOT NULL,
		[LastName] [NVARCHAR](50) NOT NULL,
		[Username] [NVARCHAR](50) NOT NULL,
		[Password] [NVARCHAR](50) NOT NULL,
		[IsAdmin] [bit] NOT NULL,
		[CreatedAt] [datetime] NOT NULL,
		[LastEdited] [datetime],
		[CreatorId] [int] NULL,
		[ModifierId] [int] NULL,
	CONSTRAINT [PK_Users] PRIMARY KEY CLUSTERED
	(
		[Id] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

CREATE TABLE [dbo].[TaskLists](
		[Id] [int] IDENTITY(1,1) NOT NULL,
		[Title] [NVARCHAR](50) NOT NULL,
		[CreatedAt] [datetime] NOT NULL,
		[LastEdited] [datetime],
		[CreatorId] [int] NOT NULL,
		[ModifierId] [int],
	CONSTRAINT [PK_Tasklists] PRIMARY KEY CLUSTERED
	(
		[Id] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

CREATE TABLE [dbo].[Tasks](
		[Id] [int] IDENTITY(1,1) NOT NULL,
		[Title] [NVARCHAR](50) NOT NULL,
		[Description] [NVARCHAR](max) NOT NULL,
		[IsCompleted] [bit] NOT NULL,
		[CreatedAt] [datetime] NOT NULL,
		[LastEdited] [datetime],
		[CreatorId] [int] NOT NULL,
		[ModifierId] [int],
		[ListId] [int] NOT NULL,
	CONSTRAINT [PK_Tasks] PRIMARY KEY CLUSTERED
	(
		[Id] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

CREATE TABLE [dbo].[UsersSharedLists](
		[UserID] [int] NOT NULL,
		[ListID] [int] NOT NULL
)

CREATE TABLE [dbo].[UsersAssignedTasks](
		[UserID] [int] NOT NULL,
		[TaskID] [int] NOT NULL
)

ALTER TABLE [dbo].[Users] ADD CONSTRAINT [DF_Users_CreatedAt] DEFAULT(getdate()) FOR [CreatedAt]

ALTER TABLE [dbo].[TaskLists] ADD CONSTRAINT [DF_TaskLists_CreatedAt] DEFAULT(getdate()) FOR [CreatedAt]

ALTER TABLE [dbo].[Tasks] ADD CONSTRAINT [DF_Tasks_CreatedAt] DEFAULT(getdate()) FOR [CreatedAt]

ALTER TABLE [dbo].[Users] WITH CHECK ADD CONSTRAINT [FK_Users_Creator] FOREIGN KEY([CreatorId])
REFERENCES [dbo].[Users] ([Id])

ALTER TABLE [dbo].[Users] CHECK CONSTRAINT [FK_Users_Creator]

ALTER TABLE [dbo].[Users] WITH CHECK ADD CONSTRAINT [FK_Users_Modifier] FOREIGN KEY([ModifierId])
REFERENCES [dbo].[Users] ([Id])

ALTER TABLE [dbo].[Users] CHECK CONSTRAINT [FK_Users_Modifier]

ALTER TABLE [dbo].[Tasks] WITH CHECK ADD CONSTRAINT [FK_Tasks_Creator] FOREIGN KEY([CreatorId])
REFERENCES [dbo].[Users] ([Id])

ALTER TABLE [dbo].[Tasks] CHECK CONSTRAINT [FK_Tasks_Creator]

ALTER TABLE [dbo].[Tasks] WITH CHECK ADD CONSTRAINT [FK_Tasks_Modifier] FOREIGN KEY([ModifierId])
REFERENCES [dbo].[Users] ([Id])

ALTER TABLE [dbo].[Tasks] CHECK CONSTRAINT [FK_Tasks_Modifier]

ALTER TABLE [dbo].[TaskLists] WITH CHECK ADD CONSTRAINT [FK_TaskLists_Creator] FOREIGN KEY([CreatorId])
REFERENCES [dbo].[Users] ([Id])

ALTER TABLE [dbo].[TaskLists] CHECK CONSTRAINT [FK_TaskLists_Creator]

ALTER TABLE [dbo].[TaskLists] WITH CHECK ADD CONSTRAINT [FK_TaskLists_Modifier] FOREIGN KEY([ModifierId])
REFERENCES [dbo].[Users] ([Id])

ALTER TABLE [dbo].[TaskLists] CHECK CONSTRAINT [FK_TaskLists_Modifier]

ALTER TABLE [dbo].[UsersSharedLists] WITH CHECK ADD CONSTRAINT [FK_UsersSharedLists_Users] FOREIGN KEY([UserId])
REFERENCES [dbo].[Users] ([Id])

ALTER TABLE [dbo].[UsersSharedLists] CHECK CONSTRAINT [FK_UsersSharedLists_Users]

ALTER TABLE [dbo].[UsersSharedLists] WITH CHECK ADD CONSTRAINT [FK_UsersSharedLists_TaskLists] FOREIGN KEY([ListId])
REFERENCES [dbo].[Tasklists] ([Id])

ALTER TABLE [dbo].[UsersSharedLists] CHECK CONSTRAINT [FK_UsersSharedLists_TaskLists]

ALTER TABLE [dbo].[UsersAssignedTasks] WITH CHECK ADD CONSTRAINT [FK_UsersAssignedTasks_Users] FOREIGN KEY([UserId])
REFERENCES [dbo].[Users] ([Id])

ALTER TABLE [dbo].[UsersAssignedTasks] CHECK CONSTRAINT [FK_UsersAssignedTasks_Users]

ALTER TABLE [dbo].[UsersAssignedTasks] WITH CHECK ADD CONSTRAINT [FK_UsersAssignedTasks_Tasks] FOREIGN KEY([TaskId])
REFERENCES [dbo].[Tasks] ([Id])

ALTER TABLE [dbo].[UsersAssignedTasks] CHECK CONSTRAINT [FK_UsersAssignedTasks_Tasks]