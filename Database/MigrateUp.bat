set ProductionProfile=1
migrate -a %1 -db %2 -conn %3 -t=migrate:up --version=30000000000000