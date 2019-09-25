Create Procedure [CustomerCode].[CustomerInfoInsert]
    @FirstName			nvarchar(50),
	@MiddleName			nvarchar(50),
	@LastName			nvarchar(50),
	@BirthDate			datetime,
	@GenderId			int,
	@CustomerTypeId	    int,
	@ActivityContextId	int = -1,
	@Key				uniqueidentifier = '00000000-0000-0000-0000-000000000000',
    @ActivityContextKey	Uniqueidentifier = '00000000-0000-0000-0000-000000000000'
AS
	-- Initialize
    Declare @Id         int = -1
	Select	@Key			= IsNull(@Key, '00000000-0000-0000-0000-000000000000')
	Select 	@FirstName		= RTRIM(LTRIM(@FirstName))
	Select 	@MiddleName		= RTRIM(LTRIM(@MiddleName))
	Select 	@LastName		= RTRIM(LTRIM(@LastName))
	
	-- Get any missing data
    If (@ActivityContextId = -1) Select @ActivityContextId = IsNull(ActivityContextId, -1) From [Activity].[ActivityContext] Where ActivityContextKey = @ActivityContextKey
	-- Validate data that will be inserted/updated, and ensure basic values exist
	If ((@FirstName <> '') Or (@MiddleName <> '') Or (@LastName <> '')) And (@ActivityContextId <> -1) And (@CustomerTypeId Is Not Null)
	Begin
		-- Id and Key are both valid. Sync now.
		If (@Id = -1 AND @Key <> '00000000-0000-0000-0000-000000000000') Select @Id = IsNull(CustomerId, -1) From [Customer].[Customer] Where CustomerKey = @Key
		-- Insert vs Update
		Begin Tran;
		Begin Try
			If (@Id Is Null) Or (@Id = -1)
			Begin
				Select @Key = IsNull(NullIf(@Key, '00000000-0000-0000-0000-000000000000'), NewId())
				Insert Into [Customer].[Customer] (CustomerKey, FirstName, MiddleName, LastName, BirthDate, GenderId, CustomerTypeId, ActivityContextKey, CreatedDate, ModifiedDate)
					Values (@Key, @FirstName, @MiddleName, @LastName, @BirthDate, @GenderId, @CustomerTypeId, @ActivityContextKey, GetUtcDate(), GetUtcDate())
				Select	@Id = SCOPE_IDENTITY()
			End
			Else
			Begin
				-- Create main entity record
				Update C
				Set C.FirstName				= @FirstName, 
					C.MiddleName			= @MiddleName, 
					C.LastName				= @LastName, 
					C.BirthDate				= @BirthDate, 
					C.GenderId				= @GenderId,
					C.CustomerTypeId		= @CustomerTypeId,
					C.ActivityContextKey	= @ActivityContextKey,
					C.ModifiedDate			= GetUTCDate()
				From	[Customer].[Customer] C
				Where	C.[CustomerId] = @Id
			End
			Commit;
		End Try
		Begin Catch
			Rollback;
			Exec [Activity].[ExceptionLogInsertByActivity] @ActivityContextId;
			Throw;
		End Catch
	End	

	-- Return data
	Select	IsNull(@Id, -1) As Id, IsNull(@Key, '00000000-0000-0000-0000-000000000000') As [Key]
