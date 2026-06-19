-- +goose Up
create table if not exists domain.events (
    aggregate_id uuid not null,
    version bigint not null,
    event_type text not null,
    payload bytea not null,
    metadata bytea not null,
    created timestamptz not null default now(),
    constraint events_pk primary key (aggregate_id, version)
);

-- +goose Down
drop table if exists domain.events;
