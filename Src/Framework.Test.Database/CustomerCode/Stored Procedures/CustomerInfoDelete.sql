Create Procedure [CustomerCode].[CustomerInfoDelete]
	@Id					int = -1,
	@ActivityContextId	int = -1,
	@Key				uniqueidentifier,
    @ActivityContextKey	Uniqueidentifier = '00000000-0000-0000-0000-000000000000'
AS
	Select	@Key	= IsNull(@Key, '00000000-0000-0000-0000-000000000000')
	Begin Tran;
	Begin Try
		-- Id and Key are both valid. Sync now.
        If (@ActivityContextId = -1) Select @ActivityContextId = IsNull(ActivityContextId, -1) From [Activity].[ActivityContext] Where ActivityContextKey = @ActivityContextKey
		If (@Id <> -1) Select @Key = IsNull(CustomerKey, '00000000-0000-0000-0000-000000000000') From [Customer].[Customer] Where CustomerId = @Id        
		If (@Id = -1 AND @Key <> '00000000-0000-0000-0000-000000000000') Select @Id = IsNull(CustomerId, -1) From [Customer].[Customer] Where CustomerKey = @Key
		If (@Id <> -1 AND @ActivityContextId <> -1)
			Delete
				From	[Customer].[Customer]
				Where	CustomerId = @Id
		Commit;
	End Try
	Begin Catch
		Rollback;
		Exec [Activity].[ExceptionLogInsertByActivity] @ActivityContextId;
		Throw;
	End Catch