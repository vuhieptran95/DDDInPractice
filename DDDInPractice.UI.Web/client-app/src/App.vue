<template>
  <v-app>
    <AppBar />
    <router-view />
    <AppFooter/>
  </v-app>
</template>

<script lang="ts">
import Vue from 'vue';
import AppBar from "@/components/AppBar.vue";
import AppFooter from "@/components/AppFooter.vue";

export default Vue.extend({
  name: 'App',

  components: {
    AppBar,
    AppFooter
  },

  data: () => ({
    //
  }),
});
</script>
