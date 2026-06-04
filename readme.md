UrbanLand — веб-приложение для ландшафтного дизайна и планировки территорий. Приложение позволяет создавать проекты застройки (парки, жилые комплексы, коттеджные посёлки и т.д.), проектировать 3D-сцены с размещением объектов, импортировать ассеты (в т.ч. через Sketchfab API), управлять доступом по ролям (Manager/Builder/Commentator/Visitor), также позволяя выдавать роли через код-приглашения.
Архитектура:
- Clean Architecture + Domain-Driven Design: 3 bounded context’а (ProjectManagement, SceneDesign, AssetCatalog) с изолированными моделями домена
- CQRS через MediatR (команды и запросы разделены)
- Event-Driven: domain events (внутри контекста) и integration events (между контекстами) через MassTransit + RabbitMQ
- Result-паттерн для всех доменных операций (Result\<T\> / Error)
- Value Object’ы, Aggregate Root’ы, репозитории, фабричные методы Create()
Стек: .NET 10 (net10.0), ASP.NET Core Minimal API, Entity Framework Core 10 + PostgreSQL (4 БД), MassTransit 9.1 + RabbitMQ, Redis, Serilog, JWT Bearer, Docker Compose
Ключевые особенности:
- 4 PostgreSQL БД (по одной на bounded context + identity) с отдельными миграциями
- MassTransit с поддержкой RabbitMQ (prod) и in-memory (dev)
- Ограничения на количество объектов на сцене (настраиваемые), детекция коллизий (bounding box)
- 2 режима просмотра: TopDown2D и Perspective3D
- Версионирование 3D-ассетов (AssetVersion) с жизненным циклом Processing → Active/Rejected → Retired
- Ролевая модель безопасности на уровне агрегата (ProjectRole с CanManageRoles/CanEditScene/CanComment)