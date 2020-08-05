﻿import {Route} from "vue-router/types/router";

export const defaultRoute: Route = {
    path: "/",
    name: null,
    hash: "",
    query: {},
    params: {},
    fullPath: "",
    matched: [],
    meta: null
};

export const RouteHelper = {
    fromRoute: defaultRoute
};

