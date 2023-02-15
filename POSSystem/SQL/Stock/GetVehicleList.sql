DECLARE @@MakID varchar(50);
DECLARE @@sqlstate varchar(max);
SET @@MakID = {0};
SET @@sqlstate = 'SELECT        dbo.MSTVehicle.ID, dbo.MSTVehicle.VehicleRegNo, dbo.MSTVehicle.VCount, dbo.MSTVehicle.VehicleType, dbo.SUVehicleType.Description AS VehicleTypeDesc, 
dbo.MSTVehicle.VehicleMake, 
                         dbo.SUVehicleMake.Description AS VehicleMakeDesc, dbo.MSTVehicle.VehicleModel, dbo.SUVehicleModel.Description AS VehicleModelDesc, dbo.MSTVehicle.FuelType, 
                         dbo.SUVehicleFuelType.Description AS FuelTypeDesc, 
                         dbo.MSTVehicle.VehicleScheme, dbo.SUVehicleScheme.Description AS VehicleSchemeDesc, dbo.MSTVehicle.ChassisNo, dbo.MSTVehicle.Engine,
                         isnull(dbo.MSTVehicle.Year,0) as Year,
                         dbo.MSTVehicle.BodyColor, 
                         dbo.MSTVehicle.VehicleDescription
FROM            dbo.MSTVehicle Left JOIN
                         dbo.SUVehicleFuelType ON dbo.MSTVehicle.FuelType = dbo.SUVehicleFuelType.Code Left JOIN
                         dbo.SUVehicleModel ON dbo.MSTVehicle.VehicleModel = dbo.SUVehicleModel.Code Left JOIN
                         dbo.SUVehicleMake ON dbo.MSTVehicle.VehicleMake = dbo.SUVehicleMake.Code Left JOIN
                         dbo.SUVehicleScheme ON dbo.MSTVehicle.VehicleScheme = dbo.SUVehicleScheme.Code Left JOIN
                         dbo.SUVehicleType ON dbo.MSTVehicle.VehicleType = dbo.SUVehicleType.Code';

--IF (@@MakID <> 0) BEGIN
--	SET @@sqlstate = @@sqlstate + ' and dbo.CourseMaintain.Mak ='+ CAST(@@MakID As varchar(20)) 
--END
execute(@@sqlstate);