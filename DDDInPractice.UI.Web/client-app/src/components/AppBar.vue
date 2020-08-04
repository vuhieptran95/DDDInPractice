﻿<template>
    <div>
        <v-navigation-drawer
                v-model="drawer"
                app
        >
            <v-list dense>
                <v-list-item :key="i" v-for="(link, i) in links" :to="link.route">
                    <v-list-item-action>
                        <v-icon>{{link.icon}}</v-icon>
                    </v-list-item-action>
                    <v-list-item-content>
                        <v-list-item-title>{{link.title}}</v-list-item-title>
                    </v-list-item-content>
                </v-list-item>
            </v-list>
        </v-navigation-drawer>
        <v-app-bar
                color="white"
                dense
                app
        >
            <v-app-bar-nav-icon @click.stop="drawer = !drawer"/>
            <v-spacer></v-spacer>
        </v-app-bar>
    </div>
</template>

<script lang="ts">
    import {Vue, Component, Prop} from 'vue-property-decorator'

    @Component
    export default class AppBar extends Vue {
        links = [
            {
                icon: "home", title: "Vending Machines", route: "/vending-machines"
            }
        ];
        drawer = null;

        mounted() {
        }

        
    }

</script>