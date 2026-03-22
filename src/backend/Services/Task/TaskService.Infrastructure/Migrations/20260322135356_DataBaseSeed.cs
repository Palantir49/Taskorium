using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace TaskService.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class DataBaseSeed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Attachments",
                keyColumn: "Id",
                keyValue: new Guid("019d15d0-2453-7457-94c0-44e362d84585"));

            migrationBuilder.DeleteData(
                table: "Attachments",
                keyColumn: "Id",
                keyValue: new Guid("019d15d0-2454-7d76-97a5-eafa1c2a255a"));

            migrationBuilder.DeleteData(
                table: "IssueStatus",
                keyColumn: "Id",
                keyValue: new Guid("019d15d0-2453-70c3-bf99-c5cd1181900c"));

            migrationBuilder.DeleteData(
                table: "IssueStatus",
                keyColumn: "Id",
                keyValue: new Guid("019d15d0-2453-7156-8d56-e8ad4e9fb210"));

            migrationBuilder.DeleteData(
                table: "IssueStatus",
                keyColumn: "Id",
                keyValue: new Guid("019d15d0-2453-733e-982b-cc29b085b551"));

            migrationBuilder.DeleteData(
                table: "IssueStatus",
                keyColumn: "Id",
                keyValue: new Guid("019d15d0-2453-7415-b462-5eb7fb31a500"));

            migrationBuilder.DeleteData(
                table: "IssueStatus",
                keyColumn: "Id",
                keyValue: new Guid("019d15d0-2453-75ce-8a81-034b0f24caea"));

            migrationBuilder.DeleteData(
                table: "IssueStatus",
                keyColumn: "Id",
                keyValue: new Guid("019d15d0-2453-7982-9a52-c2672dd071c3"));

            migrationBuilder.DeleteData(
                table: "IssueStatus",
                keyColumn: "Id",
                keyValue: new Guid("019d15d0-2453-7a14-928f-10399c976e06"));

            migrationBuilder.DeleteData(
                table: "IssueStatus",
                keyColumn: "Id",
                keyValue: new Guid("019d15d0-2453-7c75-96c8-a2df8c4c8cba"));

            migrationBuilder.DeleteData(
                table: "IssueStatus",
                keyColumn: "Id",
                keyValue: new Guid("019d15d0-2453-7d39-a748-d4f6599e3014"));

            migrationBuilder.DeleteData(
                table: "IssueStatus",
                keyColumn: "Id",
                keyValue: new Guid("019d15d0-2453-7dd0-a9d5-4853949a08ac"));

            migrationBuilder.DeleteData(
                table: "IssueType",
                keyColumn: "Id",
                keyValue: new Guid("019d15d0-2453-7067-927a-0c0cf0b0ba58"));

            migrationBuilder.DeleteData(
                table: "IssueType",
                keyColumn: "Id",
                keyValue: new Guid("019d15d0-2453-71da-a7ec-b87bbd5dc5be"));

            migrationBuilder.DeleteData(
                table: "IssueType",
                keyColumn: "Id",
                keyValue: new Guid("019d15d0-2453-73c7-ba12-42afb64f3541"));

            migrationBuilder.DeleteData(
                table: "IssueType",
                keyColumn: "Id",
                keyValue: new Guid("019d15d0-2453-790d-81fb-679d476151e5"));

            migrationBuilder.DeleteData(
                table: "IssueType",
                keyColumn: "Id",
                keyValue: new Guid("019d15d0-2453-7aa0-abb6-29a434a8748c"));

            migrationBuilder.DeleteData(
                table: "IssueType",
                keyColumn: "Id",
                keyValue: new Guid("019d15d0-2453-7d9f-bfaa-133df11a0f3d"));

            migrationBuilder.DeleteData(
                table: "IssueType",
                keyColumn: "Id",
                keyValue: new Guid("019d15d0-2453-7fa7-a25a-8e507733a5f0"));

            migrationBuilder.DeleteData(
                table: "Issues",
                keyColumn: "Id",
                keyValue: new Guid("019d15d0-2453-7003-a60f-e785b7e0a5b5"));

            migrationBuilder.DeleteData(
                table: "Issues",
                keyColumn: "Id",
                keyValue: new Guid("019d15d0-2453-71a8-9080-e3eb2f8d7daa"));

            migrationBuilder.DeleteData(
                table: "Issues",
                keyColumn: "Id",
                keyValue: new Guid("019d15d0-2453-75c4-99f6-14ce1cdeb20d"));

            migrationBuilder.DeleteData(
                table: "Issues",
                keyColumn: "Id",
                keyValue: new Guid("019d15d0-2453-77ad-b26a-21c31bfd02c6"));

            migrationBuilder.DeleteData(
                table: "Issues",
                keyColumn: "Id",
                keyValue: new Guid("019d15d0-2453-79a7-b4a1-586f179d42e7"));

            migrationBuilder.DeleteData(
                table: "Issues",
                keyColumn: "Id",
                keyValue: new Guid("019d15d0-2453-7bcd-b69d-486507755841"));

            migrationBuilder.DeleteData(
                table: "ProjectMembers",
                keyColumns: new[] { "ProjectId", "UserId" },
                keyValues: new object[] { new Guid("019d15d0-2453-77aa-a1e1-fb8792d909a0"), new Guid("019d15d0-2452-72aa-a7bd-1805328e4efa") });

            migrationBuilder.DeleteData(
                table: "ProjectMembers",
                keyColumns: new[] { "ProjectId", "UserId" },
                keyValues: new object[] { new Guid("019d15d0-2453-77aa-a1e1-fb8792d909a0"), new Guid("019d15d0-2452-7488-8f7e-1ae7d2b67617") });

            migrationBuilder.DeleteData(
                table: "ProjectMembers",
                keyColumns: new[] { "ProjectId", "UserId" },
                keyValues: new object[] { new Guid("019d15d0-2453-7a57-8998-a9493767fd5c"), new Guid("019d15d0-2452-72aa-a7bd-1805328e4efa") });

            migrationBuilder.DeleteData(
                table: "ProjectMembers",
                keyColumns: new[] { "ProjectId", "UserId" },
                keyValues: new object[] { new Guid("019d15d0-2453-7a57-8998-a9493767fd5c"), new Guid("019d15d0-2452-7488-8f7e-1ae7d2b67617") });

            migrationBuilder.DeleteData(
                table: "ProjectMembers",
                keyColumns: new[] { "ProjectId", "UserId" },
                keyValues: new object[] { new Guid("019d15d0-2453-7a57-8998-a9493767fd5c"), new Guid("019d15d0-2452-7c53-a3d0-d4c7853bc4fa") });

            migrationBuilder.DeleteData(
                table: "ProjectMembers",
                keyColumns: new[] { "ProjectId", "UserId" },
                keyValues: new object[] { new Guid("019d15d0-2453-7e73-978e-4a66d0d60d72"), new Guid("019d15d0-2452-72aa-a7bd-1805328e4efa") });

            migrationBuilder.DeleteData(
                table: "ProjectMembers",
                keyColumns: new[] { "ProjectId", "UserId" },
                keyValues: new object[] { new Guid("019d15d0-2453-7e73-978e-4a66d0d60d72"), new Guid("019d15d0-2452-7c53-a3d0-d4c7853bc4fa") });

            migrationBuilder.DeleteData(
                table: "Projects",
                keyColumn: "Id",
                keyValue: new Guid("019d15d0-2453-7446-bf75-013b1242d10d"));

            migrationBuilder.DeleteData(
                table: "Projects",
                keyColumn: "Id",
                keyValue: new Guid("019d15d0-2453-7ba7-852b-a51cfab3f744"));

            migrationBuilder.DeleteData(
                table: "Projects",
                keyColumn: "Id",
                keyValue: new Guid("019d15d0-2453-7f20-bbb4-006c9d331704"));

            migrationBuilder.DeleteData(
                table: "WorkspaceMembers",
                keyColumns: new[] { "UserId", "WorkspaceId" },
                keyValues: new object[] { new Guid("019d15d0-2452-7079-a291-fd51e37e4aa8"), new Guid("019d15d0-2452-7cc1-a65b-0f8033cc28c9") });

            migrationBuilder.DeleteData(
                table: "WorkspaceMembers",
                keyColumns: new[] { "UserId", "WorkspaceId" },
                keyValues: new object[] { new Guid("019d15d0-2452-7079-a291-fd51e37e4aa8"), new Guid("019d15d0-2453-745a-a1fe-5fc6e1fa16f3") });

            migrationBuilder.DeleteData(
                table: "WorkspaceMembers",
                keyColumns: new[] { "UserId", "WorkspaceId" },
                keyValues: new object[] { new Guid("019d15d0-2452-72aa-a7bd-1805328e4efa"), new Guid("019d15d0-2452-7cc1-a65b-0f8033cc28c9") });

            migrationBuilder.DeleteData(
                table: "WorkspaceMembers",
                keyColumns: new[] { "UserId", "WorkspaceId" },
                keyValues: new object[] { new Guid("019d15d0-2452-72aa-a7bd-1805328e4efa"), new Guid("019d15d0-2453-745a-a1fe-5fc6e1fa16f3") });

            migrationBuilder.DeleteData(
                table: "WorkspaceMembers",
                keyColumns: new[] { "UserId", "WorkspaceId" },
                keyValues: new object[] { new Guid("019d15d0-2452-7488-8f7e-1ae7d2b67617"), new Guid("019d15d0-2453-745a-a1fe-5fc6e1fa16f3") });

            migrationBuilder.DeleteData(
                table: "WorkspaceMembers",
                keyColumns: new[] { "UserId", "WorkspaceId" },
                keyValues: new object[] { new Guid("019d15d0-2452-7488-8f7e-1ae7d2b67617"), new Guid("019d15d0-2453-7e36-8ba8-9de394b7403b") });

            migrationBuilder.DeleteData(
                table: "WorkspaceMembers",
                keyColumns: new[] { "UserId", "WorkspaceId" },
                keyValues: new object[] { new Guid("019d15d0-2452-7c53-a3d0-d4c7853bc4fa"), new Guid("019d15d0-2453-745a-a1fe-5fc6e1fa16f3") });

            migrationBuilder.DeleteData(
                table: "Issues",
                keyColumn: "Id",
                keyValue: new Guid("019d15d0-2453-7233-b651-43e60963115f"));

            migrationBuilder.DeleteData(
                table: "Issues",
                keyColumn: "Id",
                keyValue: new Guid("019d15d0-2453-7429-a8bb-ebd126ee1fe2"));

            migrationBuilder.DeleteData(
                table: "Projects",
                keyColumn: "Id",
                keyValue: new Guid("019d15d0-2453-7260-8f78-11bff16a4c51"));

            migrationBuilder.DeleteData(
                table: "Projects",
                keyColumn: "Id",
                keyValue: new Guid("019d15d0-2453-77aa-a1e1-fb8792d909a0"));

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("019d15d0-2452-72aa-a7bd-1805328e4efa"));

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("019d15d0-2452-7488-8f7e-1ae7d2b67617"));

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("019d15d0-2452-7c53-a3d0-d4c7853bc4fa"));

            migrationBuilder.DeleteData(
                table: "Workspaces",
                keyColumn: "Id",
                keyValue: new Guid("019d15d0-2453-7e36-8ba8-9de394b7403b"));

            migrationBuilder.DeleteData(
                table: "Projects",
                keyColumn: "Id",
                keyValue: new Guid("019d15d0-2453-7a57-8998-a9493767fd5c"));

            migrationBuilder.DeleteData(
                table: "Projects",
                keyColumn: "Id",
                keyValue: new Guid("019d15d0-2453-7e73-978e-4a66d0d60d72"));

            migrationBuilder.DeleteData(
                table: "Workspaces",
                keyColumn: "Id",
                keyValue: new Guid("019d15d0-2452-7cc1-a65b-0f8033cc28c9"));

            migrationBuilder.DeleteData(
                table: "Workspaces",
                keyColumn: "Id",
                keyValue: new Guid("019d15d0-2453-745a-a1fe-5fc6e1fa16f3"));

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("019d15d0-2452-7079-a291-fd51e37e4aa8"));

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "CreatedDate", "Email", "FullName", "KeycloakId", "Username" },
                values: new object[,]
                {
                    { new Guid("019d15d2-e48e-7100-a0e3-aabbb5540c2b"), new DateTimeOffset(new DateTime(2026, 3, 22, 13, 53, 55, 854, DateTimeKind.Unspecified).AddTicks(6383), new TimeSpan(0, 0, 0, 0, 0)), "peter.sokolov@taskorium.local", "Peter Sokolov", new Guid("20000000-0000-0000-0000-000000000002"), "peter.sokolov" },
                    { new Guid("019d15d2-e48e-777c-98e4-745e4ecdb1bf"), new DateTimeOffset(new DateTime(2026, 3, 22, 13, 53, 55, 854, DateTimeKind.Unspecified).AddTicks(6389), new TimeSpan(0, 0, 0, 0, 0)), "alexey.nikitin@taskorium.local", "Alexey Nikitin", new Guid("40000000-0000-0000-0000-000000000004"), "alexey.nikitin" },
                    { new Guid("019d15d2-e48e-7980-a536-a85ba7fc9a9d"), new DateTimeOffset(new DateTime(2026, 3, 22, 13, 53, 55, 854, DateTimeKind.Unspecified).AddTicks(6387), new TimeSpan(0, 0, 0, 0, 0)), "kirill.volkov@taskorium.local", "Kirill Volkov", new Guid("30000000-0000-0000-0000-000000000003"), "kirill.volkov" },
                    { new Guid("019d15d2-e48e-7c9d-8b8f-48f4e825d0ee"), new DateTimeOffset(new DateTime(2026, 3, 22, 13, 53, 55, 854, DateTimeKind.Unspecified).AddTicks(5228), new TimeSpan(0, 0, 0, 0, 0)), "ivan.petrov@taskorium.local", "Ivan Petrov", new Guid("10000000-0000-0000-0000-000000000001"), "ivan.petrov" }
                });

            migrationBuilder.InsertData(
                table: "Workspaces",
                columns: new[] { "Id", "CreatedDate", "Name", "OwnerId" },
                values: new object[,]
                {
                    { new Guid("019d15d2-e48e-71f9-ba45-1194f4d963a4"), new DateTimeOffset(new DateTime(2026, 3, 22, 13, 53, 55, 854, DateTimeKind.Unspecified).AddTicks(9264), new TimeSpan(0, 0, 0, 0, 0)), "Personal", new Guid("019d15d2-e48e-7c9d-8b8f-48f4e825d0ee") },
                    { new Guid("019d15d2-e48e-72ce-81f8-543847822440"), new DateTimeOffset(new DateTime(2026, 3, 22, 13, 53, 55, 854, DateTimeKind.Unspecified).AddTicks(9761), new TimeSpan(0, 0, 0, 0, 0)), "Taskorium", new Guid("019d15d2-e48e-7c9d-8b8f-48f4e825d0ee") },
                    { new Guid("019d15d2-e48e-746b-827c-28becde8f6ad"), new DateTimeOffset(new DateTime(2026, 3, 22, 13, 53, 55, 854, DateTimeKind.Unspecified).AddTicks(9769), new TimeSpan(0, 0, 0, 0, 0)), "Sandbox", new Guid("019d15d2-e48e-7c9d-8b8f-48f4e825d0ee") }
                });

            migrationBuilder.InsertData(
                table: "Projects",
                columns: new[] { "Id", "CreatedDate", "Description", "FinishDate", "Name", "StartDate", "WorkspaceId" },
                values: new object[,]
                {
                    { new Guid("019d15d2-e48f-724a-b684-63f34426ffe9"), new DateTimeOffset(new DateTime(2026, 3, 22, 13, 53, 55, 855, DateTimeKind.Unspecified).AddTicks(2199), new TimeSpan(0, 0, 0, 0, 0)), "Time-boxed prototype: WebSocket presence and optimistic updates for the board view.", null, "spike-realtime-board", null, new Guid("019d15d2-e48e-746b-827c-28becde8f6ad") },
                    { new Guid("019d15d2-e48f-738a-b630-f0bcd663ac6e"), new DateTimeOffset(new DateTime(2026, 3, 22, 13, 53, 55, 855, DateTimeKind.Unspecified).AddTicks(2191), new TimeSpan(0, 0, 0, 0, 0)), "Pilot: lint prompts, CI gates, and review checklists for teams adopting AI-assisted coding without bypassing quality bars.", null, "ai-dev-workflow", null, new Guid("019d15d2-e48e-71f9-ba45-1194f4d963a4") },
                    { new Guid("019d15d2-e48f-759a-bb42-c42a5e1790c1"), new DateTimeOffset(new DateTime(2026, 3, 22, 13, 53, 55, 855, DateTimeKind.Unspecified).AddTicks(2195), new TimeSpan(0, 0, 0, 0, 0)), "Rewrite onboarding and API reference for the next major release; align examples with current OpenAPI spec.", null, "docs-refresh", null, new Guid("019d15d2-e48e-71f9-ba45-1194f4d963a4") },
                    { new Guid("019d15d2-e48f-7ba4-96d6-750b47b33613"), new DateTimeOffset(new DateTime(2026, 3, 22, 13, 53, 55, 855, DateTimeKind.Unspecified).AddTicks(2189), new TimeSpan(0, 0, 0, 0, 0)), "Outbox-backed worker that delivers email and in-app notifications with retry policies and idempotent handlers.", null, "taskorium-notify", null, new Guid("019d15d2-e48e-72ce-81f8-543847822440") },
                    { new Guid("019d15d2-e48f-7ccb-b352-e6174f75a6ac"), new DateTimeOffset(new DateTime(2026, 3, 22, 13, 53, 55, 855, DateTimeKind.Unspecified).AddTicks(2187), new TimeSpan(0, 0, 0, 0, 0)), "Open-source React component library shared across Taskorium frontends: layout primitives, forms, and accessibility-tested patterns.", null, "ui-kit", null, new Guid("019d15d2-e48e-72ce-81f8-543847822440") },
                    { new Guid("019d15d2-e48f-7d62-9939-75db3ee9ed74"), new DateTimeOffset(new DateTime(2026, 3, 22, 13, 53, 55, 855, DateTimeKind.Unspecified).AddTicks(2198), new TimeSpan(0, 0, 0, 0, 0)), "Track removal of deprecated endpoints and migration of remaining consumers to v2 APIs.", null, "legacy-cleanup", null, new Guid("019d15d2-e48e-746b-827c-28becde8f6ad") },
                    { new Guid("019d15d2-e48f-7d65-a334-2b43386ab02f"), new DateTimeOffset(new DateTime(2026, 3, 22, 13, 53, 55, 855, DateTimeKind.Unspecified).AddTicks(2015), new TimeSpan(0, 0, 0, 0, 0)), "ASP.NET Core service for workspaces, projects, issues, and notifications. Exposes REST APIs consumed by the web client.", null, "taskorium-api", null, new Guid("019d15d2-e48e-72ce-81f8-543847822440") }
                });

            migrationBuilder.InsertData(
                table: "WorkspaceMembers",
                columns: new[] { "UserId", "WorkspaceId", "JoinedAt", "Role" },
                values: new object[,]
                {
                    { new Guid("019d15d2-e48e-7100-a0e3-aabbb5540c2b"), new Guid("019d15d2-e48e-71f9-ba45-1194f4d963a4"), new DateTimeOffset(new DateTime(2025, 1, 8, 9, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "Member" },
                    { new Guid("019d15d2-e48e-7100-a0e3-aabbb5540c2b"), new Guid("019d15d2-e48e-72ce-81f8-543847822440"), new DateTimeOffset(new DateTime(2025, 1, 8, 9, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "Member" },
                    { new Guid("019d15d2-e48e-777c-98e4-745e4ecdb1bf"), new Guid("019d15d2-e48e-72ce-81f8-543847822440"), new DateTimeOffset(new DateTime(2025, 1, 8, 9, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "Viewer" },
                    { new Guid("019d15d2-e48e-777c-98e4-745e4ecdb1bf"), new Guid("019d15d2-e48e-746b-827c-28becde8f6ad"), new DateTimeOffset(new DateTime(2025, 1, 8, 9, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "Admin" },
                    { new Guid("019d15d2-e48e-7980-a536-a85ba7fc9a9d"), new Guid("019d15d2-e48e-72ce-81f8-543847822440"), new DateTimeOffset(new DateTime(2025, 1, 8, 9, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "Admin" },
                    { new Guid("019d15d2-e48e-7c9d-8b8f-48f4e825d0ee"), new Guid("019d15d2-e48e-71f9-ba45-1194f4d963a4"), new DateTimeOffset(new DateTime(2025, 1, 8, 9, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "Admin" },
                    { new Guid("019d15d2-e48e-7c9d-8b8f-48f4e825d0ee"), new Guid("019d15d2-e48e-72ce-81f8-543847822440"), new DateTimeOffset(new DateTime(2025, 1, 8, 9, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "Admin" }
                });

            migrationBuilder.InsertData(
                table: "IssueStatus",
                columns: new[] { "Id", "Color", "CreatedDate", "Name", "Position", "ProjectId", "Type" },
                values: new object[,]
                {
                    { new Guid("019d15d2-e48f-7030-8ad5-8422aeb59873"), "#22C55E", new DateTimeOffset(new DateTime(2026, 3, 22, 13, 53, 55, 855, DateTimeKind.Unspecified).AddTicks(7985), new TimeSpan(0, 0, 0, 0, 0)), "Done", 1, new Guid("019d15d2-e48f-7ba4-96d6-750b47b33613"), 2 },
                    { new Guid("019d15d2-e48f-71cc-8470-a635e2b811f5"), "#94A3B8", new DateTimeOffset(new DateTime(2026, 3, 22, 13, 53, 55, 855, DateTimeKind.Unspecified).AddTicks(7988), new TimeSpan(0, 0, 0, 0, 0)), "To Do", 0, new Guid("019d15d2-e48f-738a-b630-f0bcd663ac6e"), 0 },
                    { new Guid("019d15d2-e48f-7251-a42c-c5438f365e92"), "#94A3B8", new DateTimeOffset(new DateTime(2026, 3, 22, 13, 53, 55, 855, DateTimeKind.Unspecified).AddTicks(7984), new TimeSpan(0, 0, 0, 0, 0)), "To Do", 0, new Guid("019d15d2-e48f-7ba4-96d6-750b47b33613"), 0 },
                    { new Guid("019d15d2-e48f-733b-a1a0-af28c84cfc90"), "#94A3B8", new DateTimeOffset(new DateTime(2026, 3, 22, 13, 53, 55, 855, DateTimeKind.Unspecified).AddTicks(6541), new TimeSpan(0, 0, 0, 0, 0)), "To Do", 0, new Guid("019d15d2-e48f-7d65-a334-2b43386ab02f"), 0 },
                    { new Guid("019d15d2-e48f-7575-b340-a50c662e8ab6"), "#94A3B8", new DateTimeOffset(new DateTime(2026, 3, 22, 13, 53, 55, 855, DateTimeKind.Unspecified).AddTicks(7938), new TimeSpan(0, 0, 0, 0, 0)), "Backlog", 0, new Guid("019d15d2-e48f-7ccb-b352-e6174f75a6ac"), 0 },
                    { new Guid("019d15d2-e48f-77ec-bed5-90e9bf2a37da"), "#22C55E", new DateTimeOffset(new DateTime(2026, 3, 22, 13, 53, 55, 855, DateTimeKind.Unspecified).AddTicks(7078), new TimeSpan(0, 0, 0, 0, 0)), "Done", 3, new Guid("019d15d2-e48f-7d65-a334-2b43386ab02f"), 2 },
                    { new Guid("019d15d2-e48f-7905-9205-2e2b4d78cd73"), "#22C55E", new DateTimeOffset(new DateTime(2026, 3, 22, 13, 53, 55, 855, DateTimeKind.Unspecified).AddTicks(7979), new TimeSpan(0, 0, 0, 0, 0)), "Released", 1, new Guid("019d15d2-e48f-7ccb-b352-e6174f75a6ac"), 2 },
                    { new Guid("019d15d2-e48f-7a0b-bc2a-114145597528"), "#3B82F6", new DateTimeOffset(new DateTime(2026, 3, 22, 13, 53, 55, 855, DateTimeKind.Unspecified).AddTicks(7063), new TimeSpan(0, 0, 0, 0, 0)), "In Progress", 1, new Guid("019d15d2-e48f-7d65-a334-2b43386ab02f"), 1 },
                    { new Guid("019d15d2-e48f-7e3d-a478-757784a9a658"), "#A855F7", new DateTimeOffset(new DateTime(2026, 3, 22, 13, 53, 55, 855, DateTimeKind.Unspecified).AddTicks(7076), new TimeSpan(0, 0, 0, 0, 0)), "In Review", 2, new Guid("019d15d2-e48f-7d65-a334-2b43386ab02f"), 1 },
                    { new Guid("019d15d2-e48f-7f41-90a6-a80efac43691"), "#22C55E", new DateTimeOffset(new DateTime(2026, 3, 22, 13, 53, 55, 855, DateTimeKind.Unspecified).AddTicks(7989), new TimeSpan(0, 0, 0, 0, 0)), "Done", 1, new Guid("019d15d2-e48f-738a-b630-f0bcd663ac6e"), 2 }
                });

            migrationBuilder.InsertData(
                table: "IssueType",
                columns: new[] { "Id", "Color", "CreatedDate", "Name", "ProjectId" },
                values: new object[,]
                {
                    { new Guid("019d15d2-e48f-718f-b7b7-63324f85348e"), "#6366F1", new DateTimeOffset(new DateTime(2026, 3, 22, 13, 53, 55, 855, DateTimeKind.Unspecified).AddTicks(7981), new TimeSpan(0, 0, 0, 0, 0)), "Task", new Guid("019d15d2-e48f-7ccb-b352-e6174f75a6ac") },
                    { new Guid("019d15d2-e48f-746a-b5dc-0f4604b93c53"), "#0EA5E9", new DateTimeOffset(new DateTime(2026, 3, 22, 13, 53, 55, 855, DateTimeKind.Unspecified).AddTicks(7987), new TimeSpan(0, 0, 0, 0, 0)), "Task", new Guid("019d15d2-e48f-7ba4-96d6-750b47b33613") },
                    { new Guid("019d15d2-e48f-749d-bb8a-b05ff2ad1f59"), "#0EA5E9", new DateTimeOffset(new DateTime(2026, 3, 22, 13, 53, 55, 855, DateTimeKind.Unspecified).AddTicks(7932), new TimeSpan(0, 0, 0, 0, 0)), "Feature", new Guid("019d15d2-e48f-7d65-a334-2b43386ab02f") },
                    { new Guid("019d15d2-e48f-779e-971b-edf70fa2a907"), "#64748B", new DateTimeOffset(new DateTime(2026, 3, 22, 13, 53, 55, 855, DateTimeKind.Unspecified).AddTicks(7993), new TimeSpan(0, 0, 0, 0, 0)), "Task", new Guid("019d15d2-e48f-738a-b630-f0bcd663ac6e") },
                    { new Guid("019d15d2-e48f-791d-8aba-cbae4968ffad"), "#EF4444", new DateTimeOffset(new DateTime(2026, 3, 22, 13, 53, 55, 855, DateTimeKind.Unspecified).AddTicks(7745), new TimeSpan(0, 0, 0, 0, 0)), "Bug", new Guid("019d15d2-e48f-7d65-a334-2b43386ab02f") },
                    { new Guid("019d15d2-e48f-7db9-aa64-5aa7b3fd887b"), "#F97316", new DateTimeOffset(new DateTime(2026, 3, 22, 13, 53, 55, 855, DateTimeKind.Unspecified).AddTicks(7982), new TimeSpan(0, 0, 0, 0, 0)), "Bug", new Guid("019d15d2-e48f-7ccb-b352-e6174f75a6ac") },
                    { new Guid("019d15d2-e48f-7fc3-85a2-8bf40555224b"), "#64748B", new DateTimeOffset(new DateTime(2026, 3, 22, 13, 53, 55, 855, DateTimeKind.Unspecified).AddTicks(7933), new TimeSpan(0, 0, 0, 0, 0)), "Chore", new Guid("019d15d2-e48f-7d65-a334-2b43386ab02f") }
                });

            migrationBuilder.InsertData(
                table: "Issues",
                columns: new[] { "Id", "CreatedDate", "Description", "DueDate", "IssueStatusId", "IssueTypeId", "Name", "ProjectId", "ResolvedDate", "UpdatedDate" },
                values: new object[,]
                {
                    { new Guid("019d15d2-e48f-70dc-9cc0-dd168477afdd"), new DateTimeOffset(new DateTime(2026, 3, 22, 13, 53, 55, 855, DateTimeKind.Unspecified).AddTicks(9451), new TimeSpan(0, 0, 0, 0, 0)), "Generate and upload the merged spec to the docs bucket; add link from the repo README.", new DateTimeOffset(new DateTime(2025, 3, 28, 17, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("019d15d2-e48f-7e3d-a478-757784a9a658"), new Guid("019d15d2-e48f-7fc3-85a2-8bf40555224b"), "Publish OpenAPI 3.1 bundle for staging", new Guid("019d15d2-e48f-7d65-a334-2b43386ab02f"), null, null },
                    { new Guid("019d15d2-e490-708c-9fd7-33d3ee182ce7"), new DateTimeOffset(new DateTime(2026, 3, 22, 13, 53, 55, 856, DateTimeKind.Unspecified).AddTicks(47), new TimeSpan(0, 0, 0, 0, 0)), "Expose SQL and message bus connectivity in /health/detailed for k8s probes.", null, new Guid("019d15d2-e48f-77ec-bed5-90e9bf2a37da"), new Guid("019d15d2-e48f-749d-bb8a-b05ff2ad1f59"), "Health checks return dependency status", new Guid("019d15d2-e48f-7d65-a334-2b43386ab02f"), null, null },
                    { new Guid("019d15d2-e490-72de-a5ea-05dd89a5b2ab"), new DateTimeOffset(new DateTime(2026, 3, 22, 13, 53, 55, 856, DateTimeKind.Unspecified).AddTicks(70), new TimeSpan(0, 0, 0, 0, 0)), "One page: required sections, tone, and when to require human rewrite before merge.", new DateTimeOffset(new DateTime(2025, 4, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("019d15d2-e48f-71cc-8470-a635e2b811f5"), new Guid("019d15d2-e48f-779e-971b-edf70fa2a907"), "Draft team guidelines for AI-generated PR descriptions", new Guid("019d15d2-e48f-738a-b630-f0bcd663ac6e"), null, null },
                    { new Guid("019d15d2-e490-739a-9ca2-4c79498a453c"), new DateTimeOffset(new DateTime(2026, 3, 22, 13, 53, 55, 856, DateTimeKind.Unspecified).AddTicks(43), new TimeSpan(0, 0, 0, 0, 0)), "Repro: list issues with page size 50; next cursor sometimes returns 400. Add regression test against snapshot payloads.", new DateTimeOffset(new DateTime(2025, 3, 22, 12, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("019d15d2-e48f-7a0b-bc2a-114145597528"), new Guid("019d15d2-e48f-791d-8aba-cbae4968ffad"), "Fix inconsistent cursor encoding on issue list", new Guid("019d15d2-e48f-7d65-a334-2b43386ab02f"), null, null },
                    { new Guid("019d15d2-e490-798e-8d8b-961cf35f893e"), new DateTimeOffset(new DateTime(2026, 3, 22, 13, 53, 55, 856, DateTimeKind.Unspecified).AddTicks(69), new TimeSpan(0, 0, 0, 0, 0)), "Vendor scheduled March maintenance; update sealed secrets and verify staging send.", new DateTimeOffset(new DateTime(2025, 3, 25, 8, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("019d15d2-e48f-7251-a42c-c5438f365e92"), new Guid("019d15d2-e48f-746a-b5dc-0f4604b93c53"), "Rotate SMTP credentials in vault", new Guid("019d15d2-e48f-7ba4-96d6-750b47b33613"), null, null },
                    { new Guid("019d15d2-e490-7ab6-8eac-58a94f3183cb"), new DateTimeOffset(new DateTime(2026, 3, 22, 13, 53, 55, 856, DateTimeKind.Unspecified).AddTicks(50), new TimeSpan(0, 0, 0, 0, 0)), "Map legacy palette to new token names; no visual change in light theme.", null, new Guid("019d15d2-e48f-7905-9205-2e2b4d78cd73"), new Guid("019d15d2-e48f-718f-b7b7-63324f85348e"), "Ship semantic color tokens for dark mode", new Guid("019d15d2-e48f-7ccb-b352-e6174f75a6ac"), null, null },
                    { new Guid("019d15d2-e490-7d40-9d6e-609eb6eed7d3"), new DateTimeOffset(new DateTime(2026, 3, 22, 13, 53, 55, 856, DateTimeKind.Unspecified).AddTicks(48), new TimeSpan(0, 0, 0, 0, 0)), "NVDA + Chrome: tab cycle escapes modal when DatePicker overlay opens. Repro steps attached to design QA doc.", new DateTimeOffset(new DateTime(2025, 3, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("019d15d2-e48f-7575-b340-a50c662e8ab6"), new Guid("019d15d2-e48f-7db9-aa64-5aa7b3fd887b"), "Focus trap breaks inside nested Dialog", new Guid("019d15d2-e48f-7ccb-b352-e6174f75a6ac"), null, null },
                    { new Guid("019d15d2-e490-7ddf-83f6-ac114cea8e40"), new DateTimeOffset(new DateTime(2026, 3, 22, 13, 53, 55, 856, DateTimeKind.Unspecified).AddTicks(46), new TimeSpan(0, 0, 0, 0, 0)), "Align with product spec: max 8 attempts, jitter, dead-letter after final failure.", new DateTimeOffset(new DateTime(2025, 4, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("019d15d2-e48f-733b-a1a0-af28c84cfc90"), new Guid("019d15d2-e48f-749d-bb8a-b05ff2ad1f59"), "Webhook delivery retries with exponential backoff", new Guid("019d15d2-e48f-7d65-a334-2b43386ab02f"), null, null }
                });

            migrationBuilder.InsertData(
                table: "ProjectMembers",
                columns: new[] { "ProjectId", "UserId", "JoinedAt", "Role" },
                values: new object[,]
                {
                    { new Guid("019d15d2-e48f-7ba4-96d6-750b47b33613"), new Guid("019d15d2-e48e-7100-a0e3-aabbb5540c2b"), new DateTimeOffset(new DateTime(2025, 1, 8, 9, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "Member" },
                    { new Guid("019d15d2-e48f-7ba4-96d6-750b47b33613"), new Guid("019d15d2-e48e-777c-98e4-745e4ecdb1bf"), new DateTimeOffset(new DateTime(2025, 1, 8, 9, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "Admin" },
                    { new Guid("019d15d2-e48f-7ccb-b352-e6174f75a6ac"), new Guid("019d15d2-e48e-7100-a0e3-aabbb5540c2b"), new DateTimeOffset(new DateTime(2025, 1, 8, 9, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "Member" },
                    { new Guid("019d15d2-e48f-7ccb-b352-e6174f75a6ac"), new Guid("019d15d2-e48e-7980-a536-a85ba7fc9a9d"), new DateTimeOffset(new DateTime(2025, 1, 8, 9, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "Admin" },
                    { new Guid("019d15d2-e48f-7d65-a334-2b43386ab02f"), new Guid("019d15d2-e48e-7100-a0e3-aabbb5540c2b"), new DateTimeOffset(new DateTime(2025, 1, 8, 9, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "Member" },
                    { new Guid("019d15d2-e48f-7d65-a334-2b43386ab02f"), new Guid("019d15d2-e48e-777c-98e4-745e4ecdb1bf"), new DateTimeOffset(new DateTime(2025, 1, 8, 9, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "Member" },
                    { new Guid("019d15d2-e48f-7d65-a334-2b43386ab02f"), new Guid("019d15d2-e48e-7980-a536-a85ba7fc9a9d"), new DateTimeOffset(new DateTime(2025, 1, 8, 9, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "Admin" }
                });

            migrationBuilder.InsertData(
                table: "Attachments",
                columns: new[] { "Id", "IssueId", "StoragePath", "UploaderId" },
                values: new object[,]
                {
                    { new Guid("019d15d2-e490-742d-b832-781812685628"), new Guid("019d15d2-e490-7d40-9d6e-609eb6eed7d3"), "issues/ui-kit/dialog-focus/nested-dialog-repro.webm", new Guid("019d15d2-e48e-7100-a0e3-aabbb5540c2b") },
                    { new Guid("019d15d2-e490-7e75-8ec7-70074e7178ee"), new Guid("019d15d2-e48f-70dc-9cc0-dd168477afdd"), "issues/taskorium-api/openapi-staging/checklist.pdf", new Guid("019d15d2-e48e-7980-a536-a85ba7fc9a9d") }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Attachments",
                keyColumn: "Id",
                keyValue: new Guid("019d15d2-e490-742d-b832-781812685628"));

            migrationBuilder.DeleteData(
                table: "Attachments",
                keyColumn: "Id",
                keyValue: new Guid("019d15d2-e490-7e75-8ec7-70074e7178ee"));

            migrationBuilder.DeleteData(
                table: "IssueStatus",
                keyColumn: "Id",
                keyValue: new Guid("019d15d2-e48f-7030-8ad5-8422aeb59873"));

            migrationBuilder.DeleteData(
                table: "IssueStatus",
                keyColumn: "Id",
                keyValue: new Guid("019d15d2-e48f-71cc-8470-a635e2b811f5"));

            migrationBuilder.DeleteData(
                table: "IssueStatus",
                keyColumn: "Id",
                keyValue: new Guid("019d15d2-e48f-7251-a42c-c5438f365e92"));

            migrationBuilder.DeleteData(
                table: "IssueStatus",
                keyColumn: "Id",
                keyValue: new Guid("019d15d2-e48f-733b-a1a0-af28c84cfc90"));

            migrationBuilder.DeleteData(
                table: "IssueStatus",
                keyColumn: "Id",
                keyValue: new Guid("019d15d2-e48f-7575-b340-a50c662e8ab6"));

            migrationBuilder.DeleteData(
                table: "IssueStatus",
                keyColumn: "Id",
                keyValue: new Guid("019d15d2-e48f-77ec-bed5-90e9bf2a37da"));

            migrationBuilder.DeleteData(
                table: "IssueStatus",
                keyColumn: "Id",
                keyValue: new Guid("019d15d2-e48f-7905-9205-2e2b4d78cd73"));

            migrationBuilder.DeleteData(
                table: "IssueStatus",
                keyColumn: "Id",
                keyValue: new Guid("019d15d2-e48f-7a0b-bc2a-114145597528"));

            migrationBuilder.DeleteData(
                table: "IssueStatus",
                keyColumn: "Id",
                keyValue: new Guid("019d15d2-e48f-7e3d-a478-757784a9a658"));

            migrationBuilder.DeleteData(
                table: "IssueStatus",
                keyColumn: "Id",
                keyValue: new Guid("019d15d2-e48f-7f41-90a6-a80efac43691"));

            migrationBuilder.DeleteData(
                table: "IssueType",
                keyColumn: "Id",
                keyValue: new Guid("019d15d2-e48f-718f-b7b7-63324f85348e"));

            migrationBuilder.DeleteData(
                table: "IssueType",
                keyColumn: "Id",
                keyValue: new Guid("019d15d2-e48f-746a-b5dc-0f4604b93c53"));

            migrationBuilder.DeleteData(
                table: "IssueType",
                keyColumn: "Id",
                keyValue: new Guid("019d15d2-e48f-749d-bb8a-b05ff2ad1f59"));

            migrationBuilder.DeleteData(
                table: "IssueType",
                keyColumn: "Id",
                keyValue: new Guid("019d15d2-e48f-779e-971b-edf70fa2a907"));

            migrationBuilder.DeleteData(
                table: "IssueType",
                keyColumn: "Id",
                keyValue: new Guid("019d15d2-e48f-791d-8aba-cbae4968ffad"));

            migrationBuilder.DeleteData(
                table: "IssueType",
                keyColumn: "Id",
                keyValue: new Guid("019d15d2-e48f-7db9-aa64-5aa7b3fd887b"));

            migrationBuilder.DeleteData(
                table: "IssueType",
                keyColumn: "Id",
                keyValue: new Guid("019d15d2-e48f-7fc3-85a2-8bf40555224b"));

            migrationBuilder.DeleteData(
                table: "Issues",
                keyColumn: "Id",
                keyValue: new Guid("019d15d2-e490-708c-9fd7-33d3ee182ce7"));

            migrationBuilder.DeleteData(
                table: "Issues",
                keyColumn: "Id",
                keyValue: new Guid("019d15d2-e490-72de-a5ea-05dd89a5b2ab"));

            migrationBuilder.DeleteData(
                table: "Issues",
                keyColumn: "Id",
                keyValue: new Guid("019d15d2-e490-739a-9ca2-4c79498a453c"));

            migrationBuilder.DeleteData(
                table: "Issues",
                keyColumn: "Id",
                keyValue: new Guid("019d15d2-e490-798e-8d8b-961cf35f893e"));

            migrationBuilder.DeleteData(
                table: "Issues",
                keyColumn: "Id",
                keyValue: new Guid("019d15d2-e490-7ab6-8eac-58a94f3183cb"));

            migrationBuilder.DeleteData(
                table: "Issues",
                keyColumn: "Id",
                keyValue: new Guid("019d15d2-e490-7ddf-83f6-ac114cea8e40"));

            migrationBuilder.DeleteData(
                table: "ProjectMembers",
                keyColumns: new[] { "ProjectId", "UserId" },
                keyValues: new object[] { new Guid("019d15d2-e48f-7ba4-96d6-750b47b33613"), new Guid("019d15d2-e48e-7100-a0e3-aabbb5540c2b") });

            migrationBuilder.DeleteData(
                table: "ProjectMembers",
                keyColumns: new[] { "ProjectId", "UserId" },
                keyValues: new object[] { new Guid("019d15d2-e48f-7ba4-96d6-750b47b33613"), new Guid("019d15d2-e48e-777c-98e4-745e4ecdb1bf") });

            migrationBuilder.DeleteData(
                table: "ProjectMembers",
                keyColumns: new[] { "ProjectId", "UserId" },
                keyValues: new object[] { new Guid("019d15d2-e48f-7ccb-b352-e6174f75a6ac"), new Guid("019d15d2-e48e-7100-a0e3-aabbb5540c2b") });

            migrationBuilder.DeleteData(
                table: "ProjectMembers",
                keyColumns: new[] { "ProjectId", "UserId" },
                keyValues: new object[] { new Guid("019d15d2-e48f-7ccb-b352-e6174f75a6ac"), new Guid("019d15d2-e48e-7980-a536-a85ba7fc9a9d") });

            migrationBuilder.DeleteData(
                table: "ProjectMembers",
                keyColumns: new[] { "ProjectId", "UserId" },
                keyValues: new object[] { new Guid("019d15d2-e48f-7d65-a334-2b43386ab02f"), new Guid("019d15d2-e48e-7100-a0e3-aabbb5540c2b") });

            migrationBuilder.DeleteData(
                table: "ProjectMembers",
                keyColumns: new[] { "ProjectId", "UserId" },
                keyValues: new object[] { new Guid("019d15d2-e48f-7d65-a334-2b43386ab02f"), new Guid("019d15d2-e48e-777c-98e4-745e4ecdb1bf") });

            migrationBuilder.DeleteData(
                table: "ProjectMembers",
                keyColumns: new[] { "ProjectId", "UserId" },
                keyValues: new object[] { new Guid("019d15d2-e48f-7d65-a334-2b43386ab02f"), new Guid("019d15d2-e48e-7980-a536-a85ba7fc9a9d") });

            migrationBuilder.DeleteData(
                table: "Projects",
                keyColumn: "Id",
                keyValue: new Guid("019d15d2-e48f-724a-b684-63f34426ffe9"));

            migrationBuilder.DeleteData(
                table: "Projects",
                keyColumn: "Id",
                keyValue: new Guid("019d15d2-e48f-759a-bb42-c42a5e1790c1"));

            migrationBuilder.DeleteData(
                table: "Projects",
                keyColumn: "Id",
                keyValue: new Guid("019d15d2-e48f-7d62-9939-75db3ee9ed74"));

            migrationBuilder.DeleteData(
                table: "WorkspaceMembers",
                keyColumns: new[] { "UserId", "WorkspaceId" },
                keyValues: new object[] { new Guid("019d15d2-e48e-7100-a0e3-aabbb5540c2b"), new Guid("019d15d2-e48e-71f9-ba45-1194f4d963a4") });

            migrationBuilder.DeleteData(
                table: "WorkspaceMembers",
                keyColumns: new[] { "UserId", "WorkspaceId" },
                keyValues: new object[] { new Guid("019d15d2-e48e-7100-a0e3-aabbb5540c2b"), new Guid("019d15d2-e48e-72ce-81f8-543847822440") });

            migrationBuilder.DeleteData(
                table: "WorkspaceMembers",
                keyColumns: new[] { "UserId", "WorkspaceId" },
                keyValues: new object[] { new Guid("019d15d2-e48e-777c-98e4-745e4ecdb1bf"), new Guid("019d15d2-e48e-72ce-81f8-543847822440") });

            migrationBuilder.DeleteData(
                table: "WorkspaceMembers",
                keyColumns: new[] { "UserId", "WorkspaceId" },
                keyValues: new object[] { new Guid("019d15d2-e48e-777c-98e4-745e4ecdb1bf"), new Guid("019d15d2-e48e-746b-827c-28becde8f6ad") });

            migrationBuilder.DeleteData(
                table: "WorkspaceMembers",
                keyColumns: new[] { "UserId", "WorkspaceId" },
                keyValues: new object[] { new Guid("019d15d2-e48e-7980-a536-a85ba7fc9a9d"), new Guid("019d15d2-e48e-72ce-81f8-543847822440") });

            migrationBuilder.DeleteData(
                table: "WorkspaceMembers",
                keyColumns: new[] { "UserId", "WorkspaceId" },
                keyValues: new object[] { new Guid("019d15d2-e48e-7c9d-8b8f-48f4e825d0ee"), new Guid("019d15d2-e48e-71f9-ba45-1194f4d963a4") });

            migrationBuilder.DeleteData(
                table: "WorkspaceMembers",
                keyColumns: new[] { "UserId", "WorkspaceId" },
                keyValues: new object[] { new Guid("019d15d2-e48e-7c9d-8b8f-48f4e825d0ee"), new Guid("019d15d2-e48e-72ce-81f8-543847822440") });

            migrationBuilder.DeleteData(
                table: "Issues",
                keyColumn: "Id",
                keyValue: new Guid("019d15d2-e48f-70dc-9cc0-dd168477afdd"));

            migrationBuilder.DeleteData(
                table: "Issues",
                keyColumn: "Id",
                keyValue: new Guid("019d15d2-e490-7d40-9d6e-609eb6eed7d3"));

            migrationBuilder.DeleteData(
                table: "Projects",
                keyColumn: "Id",
                keyValue: new Guid("019d15d2-e48f-738a-b630-f0bcd663ac6e"));

            migrationBuilder.DeleteData(
                table: "Projects",
                keyColumn: "Id",
                keyValue: new Guid("019d15d2-e48f-7ba4-96d6-750b47b33613"));

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("019d15d2-e48e-7100-a0e3-aabbb5540c2b"));

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("019d15d2-e48e-777c-98e4-745e4ecdb1bf"));

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("019d15d2-e48e-7980-a536-a85ba7fc9a9d"));

            migrationBuilder.DeleteData(
                table: "Workspaces",
                keyColumn: "Id",
                keyValue: new Guid("019d15d2-e48e-746b-827c-28becde8f6ad"));

            migrationBuilder.DeleteData(
                table: "Projects",
                keyColumn: "Id",
                keyValue: new Guid("019d15d2-e48f-7ccb-b352-e6174f75a6ac"));

            migrationBuilder.DeleteData(
                table: "Projects",
                keyColumn: "Id",
                keyValue: new Guid("019d15d2-e48f-7d65-a334-2b43386ab02f"));

            migrationBuilder.DeleteData(
                table: "Workspaces",
                keyColumn: "Id",
                keyValue: new Guid("019d15d2-e48e-71f9-ba45-1194f4d963a4"));

            migrationBuilder.DeleteData(
                table: "Workspaces",
                keyColumn: "Id",
                keyValue: new Guid("019d15d2-e48e-72ce-81f8-543847822440"));

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("019d15d2-e48e-7c9d-8b8f-48f4e825d0ee"));

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "CreatedDate", "Email", "FullName", "KeycloakId", "Username" },
                values: new object[,]
                {
                    { new Guid("019d15d0-2452-7079-a291-fd51e37e4aa8"), new DateTimeOffset(new DateTime(2026, 3, 22, 13, 50, 55, 570, DateTimeKind.Unspecified).AddTicks(7879), new TimeSpan(0, 0, 0, 0, 0)), "ivan.petrov@taskorium.local", "Ivan Petrov", new Guid("10000000-0000-0000-0000-000000000001"), "ivan.petrov" },
                    { new Guid("019d15d0-2452-72aa-a7bd-1805328e4efa"), new DateTimeOffset(new DateTime(2026, 3, 22, 13, 50, 55, 570, DateTimeKind.Unspecified).AddTicks(8825), new TimeSpan(0, 0, 0, 0, 0)), "peter.sokolov@taskorium.local", "Peter Sokolov", new Guid("20000000-0000-0000-0000-000000000002"), "peter.sokolov" },
                    { new Guid("019d15d0-2452-7488-8f7e-1ae7d2b67617"), new DateTimeOffset(new DateTime(2026, 3, 22, 13, 50, 55, 570, DateTimeKind.Unspecified).AddTicks(8855), new TimeSpan(0, 0, 0, 0, 0)), "alexey.nikitin@taskorium.local", "Alexey Nikitin", new Guid("40000000-0000-0000-0000-000000000004"), "alexey.nikitin" },
                    { new Guid("019d15d0-2452-7c53-a3d0-d4c7853bc4fa"), new DateTimeOffset(new DateTime(2026, 3, 22, 13, 50, 55, 570, DateTimeKind.Unspecified).AddTicks(8838), new TimeSpan(0, 0, 0, 0, 0)), "kirill.volkov@taskorium.local", "Kirill Volkov", new Guid("30000000-0000-0000-0000-000000000003"), "kirill.volkov" }
                });

            migrationBuilder.InsertData(
                table: "Workspaces",
                columns: new[] { "Id", "CreatedDate", "Name", "OwnerId" },
                values: new object[,]
                {
                    { new Guid("019d15d0-2452-7cc1-a65b-0f8033cc28c9"), new DateTimeOffset(new DateTime(2026, 3, 22, 13, 50, 55, 571, DateTimeKind.Unspecified).AddTicks(675), new TimeSpan(0, 0, 0, 0, 0)), "Personal", new Guid("019d15d0-2452-7079-a291-fd51e37e4aa8") },
                    { new Guid("019d15d0-2453-745a-a1fe-5fc6e1fa16f3"), new DateTimeOffset(new DateTime(2026, 3, 22, 13, 50, 55, 571, DateTimeKind.Unspecified).AddTicks(980), new TimeSpan(0, 0, 0, 0, 0)), "Taskorium", new Guid("019d15d0-2452-7079-a291-fd51e37e4aa8") },
                    { new Guid("019d15d0-2453-7e36-8ba8-9de394b7403b"), new DateTimeOffset(new DateTime(2026, 3, 22, 13, 50, 55, 571, DateTimeKind.Unspecified).AddTicks(982), new TimeSpan(0, 0, 0, 0, 0)), "Sandbox", new Guid("019d15d0-2452-7079-a291-fd51e37e4aa8") }
                });

            migrationBuilder.InsertData(
                table: "Projects",
                columns: new[] { "Id", "CreatedDate", "Description", "FinishDate", "Name", "StartDate", "WorkspaceId" },
                values: new object[,]
                {
                    { new Guid("019d15d0-2453-7260-8f78-11bff16a4c51"), new DateTimeOffset(new DateTime(2026, 3, 22, 13, 50, 55, 571, DateTimeKind.Unspecified).AddTicks(2912), new TimeSpan(0, 0, 0, 0, 0)), "Pilot: lint prompts, CI gates, and review checklists for teams adopting AI-assisted coding without bypassing quality bars.", null, "ai-dev-workflow", null, new Guid("019d15d0-2452-7cc1-a65b-0f8033cc28c9") },
                    { new Guid("019d15d0-2453-7446-bf75-013b1242d10d"), new DateTimeOffset(new DateTime(2026, 3, 22, 13, 50, 55, 571, DateTimeKind.Unspecified).AddTicks(2929), new TimeSpan(0, 0, 0, 0, 0)), "Time-boxed prototype: WebSocket presence and optimistic updates for the board view.", null, "spike-realtime-board", null, new Guid("019d15d0-2453-7e36-8ba8-9de394b7403b") },
                    { new Guid("019d15d0-2453-77aa-a1e1-fb8792d909a0"), new DateTimeOffset(new DateTime(2026, 3, 22, 13, 50, 55, 571, DateTimeKind.Unspecified).AddTicks(2911), new TimeSpan(0, 0, 0, 0, 0)), "Outbox-backed worker that delivers email and in-app notifications with retry policies and idempotent handlers.", null, "taskorium-notify", null, new Guid("019d15d0-2453-745a-a1fe-5fc6e1fa16f3") },
                    { new Guid("019d15d0-2453-7a57-8998-a9493767fd5c"), new DateTimeOffset(new DateTime(2026, 3, 22, 13, 50, 55, 571, DateTimeKind.Unspecified).AddTicks(2777), new TimeSpan(0, 0, 0, 0, 0)), "ASP.NET Core service for workspaces, projects, issues, and notifications. Exposes REST APIs consumed by the web client.", null, "taskorium-api", null, new Guid("019d15d0-2453-745a-a1fe-5fc6e1fa16f3") },
                    { new Guid("019d15d0-2453-7ba7-852b-a51cfab3f744"), new DateTimeOffset(new DateTime(2026, 3, 22, 13, 50, 55, 571, DateTimeKind.Unspecified).AddTicks(2917), new TimeSpan(0, 0, 0, 0, 0)), "Rewrite onboarding and API reference for the next major release; align examples with current OpenAPI spec.", null, "docs-refresh", null, new Guid("019d15d0-2452-7cc1-a65b-0f8033cc28c9") },
                    { new Guid("019d15d0-2453-7e73-978e-4a66d0d60d72"), new DateTimeOffset(new DateTime(2026, 3, 22, 13, 50, 55, 571, DateTimeKind.Unspecified).AddTicks(2909), new TimeSpan(0, 0, 0, 0, 0)), "Open-source React component library shared across Taskorium frontends: layout primitives, forms, and accessibility-tested patterns.", null, "ui-kit", null, new Guid("019d15d0-2453-745a-a1fe-5fc6e1fa16f3") },
                    { new Guid("019d15d0-2453-7f20-bbb4-006c9d331704"), new DateTimeOffset(new DateTime(2026, 3, 22, 13, 50, 55, 571, DateTimeKind.Unspecified).AddTicks(2928), new TimeSpan(0, 0, 0, 0, 0)), "Track removal of deprecated endpoints and migration of remaining consumers to v2 APIs.", null, "legacy-cleanup", null, new Guid("019d15d0-2453-7e36-8ba8-9de394b7403b") }
                });

            migrationBuilder.InsertData(
                table: "WorkspaceMembers",
                columns: new[] { "UserId", "WorkspaceId", "JoinedAt", "Role" },
                values: new object[,]
                {
                    { new Guid("019d15d0-2452-7079-a291-fd51e37e4aa8"), new Guid("019d15d0-2452-7cc1-a65b-0f8033cc28c9"), new DateTimeOffset(new DateTime(2025, 1, 8, 9, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "Admin" },
                    { new Guid("019d15d0-2452-7079-a291-fd51e37e4aa8"), new Guid("019d15d0-2453-745a-a1fe-5fc6e1fa16f3"), new DateTimeOffset(new DateTime(2025, 1, 8, 9, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "Admin" },
                    { new Guid("019d15d0-2452-72aa-a7bd-1805328e4efa"), new Guid("019d15d0-2452-7cc1-a65b-0f8033cc28c9"), new DateTimeOffset(new DateTime(2025, 1, 8, 9, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "Member" },
                    { new Guid("019d15d0-2452-72aa-a7bd-1805328e4efa"), new Guid("019d15d0-2453-745a-a1fe-5fc6e1fa16f3"), new DateTimeOffset(new DateTime(2025, 1, 8, 9, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "Member" },
                    { new Guid("019d15d0-2452-7488-8f7e-1ae7d2b67617"), new Guid("019d15d0-2453-745a-a1fe-5fc6e1fa16f3"), new DateTimeOffset(new DateTime(2025, 1, 8, 9, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "Viewer" },
                    { new Guid("019d15d0-2452-7488-8f7e-1ae7d2b67617"), new Guid("019d15d0-2453-7e36-8ba8-9de394b7403b"), new DateTimeOffset(new DateTime(2025, 1, 8, 9, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "Admin" },
                    { new Guid("019d15d0-2452-7c53-a3d0-d4c7853bc4fa"), new Guid("019d15d0-2453-745a-a1fe-5fc6e1fa16f3"), new DateTimeOffset(new DateTime(2025, 1, 8, 9, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "Admin" }
                });

            migrationBuilder.InsertData(
                table: "IssueStatus",
                columns: new[] { "Id", "Color", "CreatedDate", "Name", "Position", "ProjectId", "Type" },
                values: new object[,]
                {
                    { new Guid("019d15d0-2453-70c3-bf99-c5cd1181900c"), "#22C55E", new DateTimeOffset(new DateTime(2026, 3, 22, 13, 50, 55, 571, DateTimeKind.Unspecified).AddTicks(7859), new TimeSpan(0, 0, 0, 0, 0)), "Done", 1, new Guid("019d15d0-2453-77aa-a1e1-fb8792d909a0"), 2 },
                    { new Guid("019d15d0-2453-7156-8d56-e8ad4e9fb210"), "#94A3B8", new DateTimeOffset(new DateTime(2026, 3, 22, 13, 50, 55, 571, DateTimeKind.Unspecified).AddTicks(7858), new TimeSpan(0, 0, 0, 0, 0)), "To Do", 0, new Guid("019d15d0-2453-77aa-a1e1-fb8792d909a0"), 0 },
                    { new Guid("019d15d0-2453-733e-982b-cc29b085b551"), "#94A3B8", new DateTimeOffset(new DateTime(2026, 3, 22, 13, 50, 55, 571, DateTimeKind.Unspecified).AddTicks(6663), new TimeSpan(0, 0, 0, 0, 0)), "To Do", 0, new Guid("019d15d0-2453-7a57-8998-a9493767fd5c"), 0 },
                    { new Guid("019d15d0-2453-7415-b462-5eb7fb31a500"), "#3B82F6", new DateTimeOffset(new DateTime(2026, 3, 22, 13, 50, 55, 571, DateTimeKind.Unspecified).AddTicks(7096), new TimeSpan(0, 0, 0, 0, 0)), "In Progress", 1, new Guid("019d15d0-2453-7a57-8998-a9493767fd5c"), 1 },
                    { new Guid("019d15d0-2453-75ce-8a81-034b0f24caea"), "#22C55E", new DateTimeOffset(new DateTime(2026, 3, 22, 13, 50, 55, 571, DateTimeKind.Unspecified).AddTicks(7100), new TimeSpan(0, 0, 0, 0, 0)), "Done", 3, new Guid("019d15d0-2453-7a57-8998-a9493767fd5c"), 2 },
                    { new Guid("019d15d0-2453-7982-9a52-c2672dd071c3"), "#22C55E", new DateTimeOffset(new DateTime(2026, 3, 22, 13, 50, 55, 571, DateTimeKind.Unspecified).AddTicks(7854), new TimeSpan(0, 0, 0, 0, 0)), "Released", 1, new Guid("019d15d0-2453-7e73-978e-4a66d0d60d72"), 2 },
                    { new Guid("019d15d0-2453-7a14-928f-10399c976e06"), "#94A3B8", new DateTimeOffset(new DateTime(2026, 3, 22, 13, 50, 55, 571, DateTimeKind.Unspecified).AddTicks(7863), new TimeSpan(0, 0, 0, 0, 0)), "To Do", 0, new Guid("019d15d0-2453-7260-8f78-11bff16a4c51"), 0 },
                    { new Guid("019d15d0-2453-7c75-96c8-a2df8c4c8cba"), "#22C55E", new DateTimeOffset(new DateTime(2026, 3, 22, 13, 50, 55, 571, DateTimeKind.Unspecified).AddTicks(7864), new TimeSpan(0, 0, 0, 0, 0)), "Done", 1, new Guid("019d15d0-2453-7260-8f78-11bff16a4c51"), 2 },
                    { new Guid("019d15d0-2453-7d39-a748-d4f6599e3014"), "#94A3B8", new DateTimeOffset(new DateTime(2026, 3, 22, 13, 50, 55, 571, DateTimeKind.Unspecified).AddTicks(7852), new TimeSpan(0, 0, 0, 0, 0)), "Backlog", 0, new Guid("019d15d0-2453-7e73-978e-4a66d0d60d72"), 0 },
                    { new Guid("019d15d0-2453-7dd0-a9d5-4853949a08ac"), "#A855F7", new DateTimeOffset(new DateTime(2026, 3, 22, 13, 50, 55, 571, DateTimeKind.Unspecified).AddTicks(7098), new TimeSpan(0, 0, 0, 0, 0)), "In Review", 2, new Guid("019d15d0-2453-7a57-8998-a9493767fd5c"), 1 }
                });

            migrationBuilder.InsertData(
                table: "IssueType",
                columns: new[] { "Id", "Color", "CreatedDate", "Name", "ProjectId" },
                values: new object[,]
                {
                    { new Guid("019d15d0-2453-7067-927a-0c0cf0b0ba58"), "#64748B", new DateTimeOffset(new DateTime(2026, 3, 22, 13, 50, 55, 571, DateTimeKind.Unspecified).AddTicks(7865), new TimeSpan(0, 0, 0, 0, 0)), "Task", new Guid("019d15d0-2453-7260-8f78-11bff16a4c51") },
                    { new Guid("019d15d0-2453-71da-a7ec-b87bbd5dc5be"), "#0EA5E9", new DateTimeOffset(new DateTime(2026, 3, 22, 13, 50, 55, 571, DateTimeKind.Unspecified).AddTicks(7862), new TimeSpan(0, 0, 0, 0, 0)), "Task", new Guid("019d15d0-2453-77aa-a1e1-fb8792d909a0") },
                    { new Guid("019d15d0-2453-73c7-ba12-42afb64f3541"), "#0EA5E9", new DateTimeOffset(new DateTime(2026, 3, 22, 13, 50, 55, 571, DateTimeKind.Unspecified).AddTicks(7846), new TimeSpan(0, 0, 0, 0, 0)), "Feature", new Guid("019d15d0-2453-7a57-8998-a9493767fd5c") },
                    { new Guid("019d15d0-2453-790d-81fb-679d476151e5"), "#6366F1", new DateTimeOffset(new DateTime(2026, 3, 22, 13, 50, 55, 571, DateTimeKind.Unspecified).AddTicks(7855), new TimeSpan(0, 0, 0, 0, 0)), "Task", new Guid("019d15d0-2453-7e73-978e-4a66d0d60d72") },
                    { new Guid("019d15d0-2453-7aa0-abb6-29a434a8748c"), "#EF4444", new DateTimeOffset(new DateTime(2026, 3, 22, 13, 50, 55, 571, DateTimeKind.Unspecified).AddTicks(7710), new TimeSpan(0, 0, 0, 0, 0)), "Bug", new Guid("019d15d0-2453-7a57-8998-a9493767fd5c") },
                    { new Guid("019d15d0-2453-7d9f-bfaa-133df11a0f3d"), "#64748B", new DateTimeOffset(new DateTime(2026, 3, 22, 13, 50, 55, 571, DateTimeKind.Unspecified).AddTicks(7847), new TimeSpan(0, 0, 0, 0, 0)), "Chore", new Guid("019d15d0-2453-7a57-8998-a9493767fd5c") },
                    { new Guid("019d15d0-2453-7fa7-a25a-8e507733a5f0"), "#F97316", new DateTimeOffset(new DateTime(2026, 3, 22, 13, 50, 55, 571, DateTimeKind.Unspecified).AddTicks(7856), new TimeSpan(0, 0, 0, 0, 0)), "Bug", new Guid("019d15d0-2453-7e73-978e-4a66d0d60d72") }
                });

            migrationBuilder.InsertData(
                table: "Issues",
                columns: new[] { "Id", "CreatedDate", "Description", "DueDate", "IssueStatusId", "IssueTypeId", "Name", "ProjectId", "ResolvedDate", "UpdatedDate" },
                values: new object[,]
                {
                    { new Guid("019d15d0-2453-7003-a60f-e785b7e0a5b5"), new DateTimeOffset(new DateTime(2026, 3, 22, 13, 50, 55, 571, DateTimeKind.Unspecified).AddTicks(9344), new TimeSpan(0, 0, 0, 0, 0)), "Vendor scheduled March maintenance; update sealed secrets and verify staging send.", new DateTimeOffset(new DateTime(2025, 3, 25, 8, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("019d15d0-2453-7156-8d56-e8ad4e9fb210"), new Guid("019d15d0-2453-71da-a7ec-b87bbd5dc5be"), "Rotate SMTP credentials in vault", new Guid("019d15d0-2453-77aa-a1e1-fb8792d909a0"), null, null },
                    { new Guid("019d15d0-2453-71a8-9080-e3eb2f8d7daa"), new DateTimeOffset(new DateTime(2026, 3, 22, 13, 50, 55, 571, DateTimeKind.Unspecified).AddTicks(9345), new TimeSpan(0, 0, 0, 0, 0)), "One page: required sections, tone, and when to require human rewrite before merge.", new DateTimeOffset(new DateTime(2025, 4, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("019d15d0-2453-7a14-928f-10399c976e06"), new Guid("019d15d0-2453-7067-927a-0c0cf0b0ba58"), "Draft team guidelines for AI-generated PR descriptions", new Guid("019d15d0-2453-7260-8f78-11bff16a4c51"), null, null },
                    { new Guid("019d15d0-2453-7233-b651-43e60963115f"), new DateTimeOffset(new DateTime(2026, 3, 22, 13, 50, 55, 571, DateTimeKind.Unspecified).AddTicks(8823), new TimeSpan(0, 0, 0, 0, 0)), "Generate and upload the merged spec to the docs bucket; add link from the repo README.", new DateTimeOffset(new DateTime(2025, 3, 28, 17, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("019d15d0-2453-7dd0-a9d5-4853949a08ac"), new Guid("019d15d0-2453-7d9f-bfaa-133df11a0f3d"), "Publish OpenAPI 3.1 bundle for staging", new Guid("019d15d0-2453-7a57-8998-a9493767fd5c"), null, null },
                    { new Guid("019d15d0-2453-7429-a8bb-ebd126ee1fe2"), new DateTimeOffset(new DateTime(2026, 3, 22, 13, 50, 55, 571, DateTimeKind.Unspecified).AddTicks(9331), new TimeSpan(0, 0, 0, 0, 0)), "NVDA + Chrome: tab cycle escapes modal when DatePicker overlay opens. Repro steps attached to design QA doc.", new DateTimeOffset(new DateTime(2025, 3, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("019d15d0-2453-7d39-a748-d4f6599e3014"), new Guid("019d15d0-2453-7fa7-a25a-8e507733a5f0"), "Focus trap breaks inside nested Dialog", new Guid("019d15d0-2453-7e73-978e-4a66d0d60d72"), null, null },
                    { new Guid("019d15d0-2453-75c4-99f6-14ce1cdeb20d"), new DateTimeOffset(new DateTime(2026, 3, 22, 13, 50, 55, 571, DateTimeKind.Unspecified).AddTicks(9326), new TimeSpan(0, 0, 0, 0, 0)), "Align with product spec: max 8 attempts, jitter, dead-letter after final failure.", new DateTimeOffset(new DateTime(2025, 4, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("019d15d0-2453-733e-982b-cc29b085b551"), new Guid("019d15d0-2453-73c7-ba12-42afb64f3541"), "Webhook delivery retries with exponential backoff", new Guid("019d15d0-2453-7a57-8998-a9493767fd5c"), null, null },
                    { new Guid("019d15d0-2453-77ad-b26a-21c31bfd02c6"), new DateTimeOffset(new DateTime(2026, 3, 22, 13, 50, 55, 571, DateTimeKind.Unspecified).AddTicks(9324), new TimeSpan(0, 0, 0, 0, 0)), "Repro: list issues with page size 50; next cursor sometimes returns 400. Add regression test against snapshot payloads.", new DateTimeOffset(new DateTime(2025, 3, 22, 12, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("019d15d0-2453-7415-b462-5eb7fb31a500"), new Guid("019d15d0-2453-7aa0-abb6-29a434a8748c"), "Fix inconsistent cursor encoding on issue list", new Guid("019d15d0-2453-7a57-8998-a9493767fd5c"), null, null },
                    { new Guid("019d15d0-2453-79a7-b4a1-586f179d42e7"), new DateTimeOffset(new DateTime(2026, 3, 22, 13, 50, 55, 571, DateTimeKind.Unspecified).AddTicks(9333), new TimeSpan(0, 0, 0, 0, 0)), "Map legacy palette to new token names; no visual change in light theme.", null, new Guid("019d15d0-2453-7982-9a52-c2672dd071c3"), new Guid("019d15d0-2453-790d-81fb-679d476151e5"), "Ship semantic color tokens for dark mode", new Guid("019d15d0-2453-7e73-978e-4a66d0d60d72"), null, null },
                    { new Guid("019d15d0-2453-7bcd-b69d-486507755841"), new DateTimeOffset(new DateTime(2026, 3, 22, 13, 50, 55, 571, DateTimeKind.Unspecified).AddTicks(9327), new TimeSpan(0, 0, 0, 0, 0)), "Expose SQL and message bus connectivity in /health/detailed for k8s probes.", null, new Guid("019d15d0-2453-75ce-8a81-034b0f24caea"), new Guid("019d15d0-2453-73c7-ba12-42afb64f3541"), "Health checks return dependency status", new Guid("019d15d0-2453-7a57-8998-a9493767fd5c"), null, null }
                });

            migrationBuilder.InsertData(
                table: "ProjectMembers",
                columns: new[] { "ProjectId", "UserId", "JoinedAt", "Role" },
                values: new object[,]
                {
                    { new Guid("019d15d0-2453-77aa-a1e1-fb8792d909a0"), new Guid("019d15d0-2452-72aa-a7bd-1805328e4efa"), new DateTimeOffset(new DateTime(2025, 1, 8, 9, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "Member" },
                    { new Guid("019d15d0-2453-77aa-a1e1-fb8792d909a0"), new Guid("019d15d0-2452-7488-8f7e-1ae7d2b67617"), new DateTimeOffset(new DateTime(2025, 1, 8, 9, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "Admin" },
                    { new Guid("019d15d0-2453-7a57-8998-a9493767fd5c"), new Guid("019d15d0-2452-72aa-a7bd-1805328e4efa"), new DateTimeOffset(new DateTime(2025, 1, 8, 9, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "Member" },
                    { new Guid("019d15d0-2453-7a57-8998-a9493767fd5c"), new Guid("019d15d0-2452-7488-8f7e-1ae7d2b67617"), new DateTimeOffset(new DateTime(2025, 1, 8, 9, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "Member" },
                    { new Guid("019d15d0-2453-7a57-8998-a9493767fd5c"), new Guid("019d15d0-2452-7c53-a3d0-d4c7853bc4fa"), new DateTimeOffset(new DateTime(2025, 1, 8, 9, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "Admin" },
                    { new Guid("019d15d0-2453-7e73-978e-4a66d0d60d72"), new Guid("019d15d0-2452-72aa-a7bd-1805328e4efa"), new DateTimeOffset(new DateTime(2025, 1, 8, 9, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "Member" },
                    { new Guid("019d15d0-2453-7e73-978e-4a66d0d60d72"), new Guid("019d15d0-2452-7c53-a3d0-d4c7853bc4fa"), new DateTimeOffset(new DateTime(2025, 1, 8, 9, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "Admin" }
                });

            migrationBuilder.InsertData(
                table: "Attachments",
                columns: new[] { "Id", "IssueId", "StoragePath", "UploaderId" },
                values: new object[,]
                {
                    { new Guid("019d15d0-2453-7457-94c0-44e362d84585"), new Guid("019d15d0-2453-7233-b651-43e60963115f"), "issues/taskorium-api/openapi-staging/checklist.pdf", new Guid("019d15d0-2452-7c53-a3d0-d4c7853bc4fa") },
                    { new Guid("019d15d0-2454-7d76-97a5-eafa1c2a255a"), new Guid("019d15d0-2453-7429-a8bb-ebd126ee1fe2"), "issues/ui-kit/dialog-focus/nested-dialog-repro.webm", new Guid("019d15d0-2452-72aa-a7bd-1805328e4efa") }
                });
        }
    }
}
