import { createRouter, createWebHistory } from 'vue-router'
import PatientList from '../views/PatientList.vue'
import PatientDetail from '../views/PatientDetail.vue'
import DoctorPlanning from '../views/DoctorPlanning.vue'
import DepartmentStats from '../views/DepartmentStats.vue'

const router = createRouter({
    history: createWebHistory(),
    routes: [
        { path: '/', redirect: '/patients' },
        { path: '/patients', component: PatientList },
        { path: '/patients/:id', component: PatientDetail },
        { path: '/doctor/:id/planning', component: DoctorPlanning },
        { path: '/departments/stats', component: DepartmentStats }
    ]
})

export default router