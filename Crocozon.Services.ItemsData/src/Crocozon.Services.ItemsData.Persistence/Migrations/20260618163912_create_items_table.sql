-- +goose Up
create table if not exists public.items(
    item_id             bigint          primary key,
    name                text            not null,
    base_price          decimal(18, 2)  not null,
    base_price_currency varchar(10)     not null
);

-- +goose Down
drop table if exists public.items;
