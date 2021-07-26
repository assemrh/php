/****** Script for SelectTopNRows command from SSMS  ******/
SELECT TOP (1000) [id]
      ,[description]
      ,[details]
      ,[referal_id]
      ,[referal_type]
      ,[start_date]
      ,[end_date]
      ,[discount_percentage]
      ,[created_at]
      ,[updated_at]
      ,[name]
      ,[old_price]
      ,[paymentmethods]
      ,[address_id]
      ,[mobile]
      ,[website]
  FROM [le-garage].[dbo].[Offers];


--alter table Offers

ALTER TABLE Offers
DROP COLUMN  details, old_price  ;


CREATE TABLE Products_Offers  (
    [Id] uniqueidentifier PRIMARY KEY NOT NULL,
    [Product_id] uniqueidentifier ,
    [offer_id] uniqueidentifier,    
    [created_at] datetime,
    [updated_at] datetime
);

CREATE TABLE Categories_Offers  (
    [Id] uniqueidentifier PRIMARY KEY NOT NULL,
    [category_id] uniqueidentifier ,
    [offer_id] uniqueidentifier ,    
    [created_at] datetime,
    [updated_at] datetime
);

CREATE TABLE Offers_Models  (
    [Id] uniqueidentifier PRIMARY KEY NOT NULL,
    [model_id] uniqueidentifier,
    [offer_id] uniqueidentifier,    
    [created_at] datetime,
    [updated_at] datetime
);

CREATE TABLE Offers_Brands  (
    [Id] uniqueidentifier PRIMARY KEY NOT NULL,
    [brand_id] uniqueidentifier,
    [offer_id] uniqueidentifier,    
    [created_at] datetime,
    [updated_at] datetime
);

