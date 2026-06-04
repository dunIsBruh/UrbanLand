-- Создаем базы данных
CREATE DATABASE urban_land_identity OWNER admin;
CREATE DATABASE urban_land_project_management OWNER admin;
CREATE DATABASE urban_land_scene_design OWNER admin;
CREATE DATABASE urban_land_asset_catalog OWNER admin;

-- Выдаем права
GRANT ALL PRIVILEGES ON DATABASE urban_land_identity TO admin;
GRANT ALL PRIVILEGES ON DATABASE urban_land_project_management TO admin;
GRANT ALL PRIVILEGES ON DATABASE urban_land_scene_design TO admin;
GRANT ALL PRIVILEGES ON DATABASE urban_land_asset_catalog TO admin;
