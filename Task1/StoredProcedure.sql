DELIMITER //
DROP PROCEDURE IF EXISTS CalculateSumAndMedian; --для соблюдения условия повторяемости

CREATE PROCEDURE CalculateSumAndMedian(OUT totalSum BIGINT, OUT medianDecimal DECIMAL(10, 8))
BEGIN

    DECLARE totalCount INT;
    DECLARE limitValue INT;
    DECLARE offsetValue INT;

    SELECT SUM(EvenInteger) INTO totalSum FROM ImportedData;

    SELECT COUNT(*) INTO totalCount FROM ImportedData;

    SET limitValue = 2 - (totalCount % 2);  
    SET offsetValue = (totalCount - 1) / 2;

    SELECT AVG(DecimalNumber) INTO medianDecimal
    FROM (
        SELECT DecimalNumber
        FROM ImportedData
        ORDER BY DecimalNumber
        LIMIT limitValue OFFSET offsetValue
    ) AS medianSubquery;
    
END //
DELIMITER ;

CALL CalculateSumAndMedian(@totalSum, @medianDecimal);
SELECT @totalSum, @medianDecimal;