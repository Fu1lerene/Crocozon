-- +goose Up
create schema if not exists domain;

-- +goose Down
drop schema if exists domain;
