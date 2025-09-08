CREATE FUNCTION dbo.CalculateDistanceKm
(
    @Lat1 DECIMAL(9,6),
    @Lon1 DECIMAL(9,6),
    @Lat2 DECIMAL(9,6),
    @Lon2 DECIMAL(9,6)
)
    RETURNS FLOAT
AS
BEGIN
    DECLARE @EarthRadiusKm FLOAT = 6371.0;

    DECLARE @Lat1Rad FLOAT = RADIANS(@Lat1);
    DECLARE @Lat2Rad FLOAT = RADIANS(@Lat2);
    DECLARE @DeltaLat FLOAT = RADIANS(@Lat2 - @Lat1);
    DECLARE @DeltaLon FLOAT = RADIANS(@Lon2 - @Lon1);

    DECLARE @A FLOAT =
        SIN(@DeltaLat / 2) * SIN(@DeltaLat / 2) +
        COS(@Lat1Rad) * COS(@Lat2Rad) *
        SIN(@DeltaLon / 2) * SIN(@DeltaLon / 2);

    DECLARE @C FLOAT = 2 * ATN2(SQRT(@A), SQRT(1 - @A));

RETURN @EarthRadiusKm * @C;
END
