alter proc InsertLeadList      
(      
@ContentTable LeadFileUploadSaveType readonly,      
@UserId int,      
@Result int out      
)      
as      
begin      
      
  declare @Code nvarchar(100) ,@Id int                                        
      
declare @Date datetime =NULL, @Campaign nvarchar(max) =NULL, @CompanyName nvarchar(max) =NULL,      
 @ContactPersonName nvarchar(max) =NULL,@CountryCodeContactNumber  nvarchar(max)= NULL,@ContactNumber nvarchar(max) =NULL,      
  @ContactPersondesignation nvarchar(max)= NULL,      
 @LandPhoneNoCountryCode nvarchar(max) =NULL, @LandPhoneNo nvarchar(max)= NULL,      
 @Email nvarchar(max)= NULL, @Website nvarchar(max) =NULL,      
 @Activity nvarchar(max)= NULL, @ActivityDescription nvarchar(max) =NULL,      
  @CustomerResponse nvarchar(max) =NULL,      
 @LeadSourceId int, @AssignedEmployeeId int,      
 @PriorityId int, @SegmentId int,@CityId int      
      
      
begin try                                                                                                                                    
 begin transaction        
      
declare cur_details  cursor       
    for       
   select * from @ContentTable      
    open cur_details       
   fetch next from cur_details into @Date ,@Campaign,@CompanyName,@ContactPersonName ,@CountryCodeContactNumber,@ContactNumber,      
    @ContactPersondesignation ,@LandPhoneNoCountryCode , @LandPhoneNo ,@Email , @Website   ,      
 @Activity, @ActivityDescription  , @CustomerResponse ,@LeadSourceId,@AssignedEmployeeId,      
 @PriorityId,@SegmentId,@CityId      
        
   while @@FETCH_STATUS=0       
   begin        
      
    set @Code=(Select ISNULL(prefix,'')+ISNULL(seperator,' ')+REPLACE(STR(current_no,Lengths),' ','0')       
 from SequenceNumber where FormId=138)       
      
  insert into Lead(code,LeadDate ,Campaign,CompanyName,ContactPersonName , ContactPersonDesig ,      
 CountryCodeCN,MobileNumber ,CountryCodeLPN, LandPhoneNo ,EmailId , LeadBrand ,      
 Activity, [Address]  , CustomerResponse,LeadSourceId , AssignedEmployeeId ,      
 Priority, SegmentId,CityId,Status  )      
 values      
 (@Code,@Date,@Campaign ,@CompanyName,@ContactPersonName , @ContactPersondesignation ,      
 @CountryCodeContactNumber,@ContactNumber ,@LandPhoneNoCountryCode, @LandPhoneNo ,@Email , @Website ,      
 @Activity, @ActivityDescription  , @CustomerResponse,@LeadSourceId , @AssignedEmployeeId ,      
 @PriorityId , @SegmentId,@CityId,1 )      
      
 set @Id=@@IDENTITY                                 
      
      
     update SequenceNumber set Current_no=Current_no+Incrementer where Formid=138            
      
 --leadfollowup                            
   Insert into LeadFollowup(LeadId,Date, Status,CustomerResponse,Remark,CreatedBy)                                                                                                              
 values( @Id,@Date ,1,@CustomerResponse,'New Lead Created',@UserId)       
      
   fetch next from cur_details into @Date ,@Campaign,@CompanyName,@ContactPersonName ,@CountryCodeContactNumber,@ContactNumber,      
    @ContactPersondesignation ,@LandPhoneNoCountryCode , @LandPhoneNo ,@Email , @Website   ,      
 @Activity, @ActivityDescription  , @CustomerResponse ,@LeadSourceId,@AssignedEmployeeId,      
 @PriorityId,@SegmentId,@CityId      
      
   end       
   close cur_details       
   deallocate cur_details      
      
 set @Result=1       
 commit transaction                            
end try                                             
begin catch                          
 rollback transaction                                                           
 set @Result=0                                                                                                                                    
end catch             
      
end

go