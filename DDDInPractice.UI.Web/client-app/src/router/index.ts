import Vue from 'vue'
import VueRouter from 'vue-router'
import Home from "@/views/Home.vue";
import VendingMachines from "@/components/VendingMachines.vue";

Vue.use(VueRouter);

const routes = [
    {
        path: '/',
        name: 'home',
        component: Home,
    },
    {
        path: '/vending-machines',
        name: 'vending-machines',
        component: VendingMachines
    }
]

const router = new VueRouter({
    mode: 'history',
    base: process.env.BASE_URL,
    routes
})

export default router
