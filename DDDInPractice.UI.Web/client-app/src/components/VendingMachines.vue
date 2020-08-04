<template>
    <v-content>
        <v-container>
            <v-row>
                <v-col>
                    <h1>These are vending machines</h1>
                </v-col>
            </v-row>
        </v-container>
    </v-content>
</template>

<script lang="ts">
    import {Vue, Component} from 'vue-property-decorator';
    import VueHelper from "@/helpers/VueHelper";
    import axios from "axios";
    import _ from "lodash";

    @Component
    export default class VendingMachines extends Vue {
        
    }
</script>
