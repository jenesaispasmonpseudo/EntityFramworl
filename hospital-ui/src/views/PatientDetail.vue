<template>
  <div>
    <RouterLink to="/patients">← Retour à la liste</RouterLink>

    <div v-if="patient">
      <h1>{{ patient.firstName }} {{ patient.lastName }}</h1>
      <div class="info-card">
        <p><strong>N° Dossier :</strong> {{ patient.fileNumber }}</p>
        <p><strong>Email :</strong> {{ patient.email }}</p>
      </div>

      <h2>Consultations</h2>
      <table v-if="patient.consultations && patient.consultations.length > 0">
        <thead>
          <tr>
            <th>Date</th>
            <th>Médecin</th>
            <th>Spécialité</th>
            <th>Statut</th>
            <th>Notes</th>
          </tr>
        </thead>
        <tbody>
          <tr v-for="c in patient.consultations" :key="c.id">
            <td>{{ formatDate(c.date) }}</td>
            <td>{{ c.doctorFullName }}</td>
            <td>{{ c.doctorSpecialty }}</td>
            <td><span :class="statusClass(c.status)">{{ c.status }}</span></td>
            <td>{{ c.notes || '-' }}</td>
          </tr>
        </tbody>
      </table>
      <p v-else>Aucune consultation enregistrée.</p>
    </div>

    <p v-else>Chargement...</p>
  </div>
</template>

<script>
import api from '../api/api'

export default {
  data() {
    return { patient: null }
  },
  mounted() {
    this.loadPatient()
  },
  methods: {
    async loadPatient() {
      const response = await api.get(`/Dashboard/patient/${this.$route.params.id}`)
      this.patient = response.data
    },
    formatDate(date) {
      return new Date(date).toLocaleDateString('fr-FR', {
        day: '2-digit', month: '2-digit', year: 'numeric',
        hour: '2-digit', minute: '2-digit'
      })
    },
    statusClass(status) {
      return {
        'status-planned': status === 'Planned',
        'status-done': status === 'Done',
        'status-cancelled': status === 'Cancelled'
      }
    }
  }
}
</script>

<style scoped>
table { width: 100%; border-collapse: collapse; margin-top: 1rem; }
th, td { padding: 0.75rem; border: 1px solid #ddd; text-align: left; }
th { background: #2c3e50; color: white; }
.info-card { background: #f8f9fa; padding: 1rem; border-radius: 8px; margin: 1rem 0; }
.status-planned  { background: #3498db; color: white; padding: 2px 8px; border-radius: 4px; }
.status-done     { background: #2ecc71; color: white; padding: 2px 8px; border-radius: 4px; }
.status-cancelled{ background: #e74c3c; color: white; padding: 2px 8px; border-radius: 4px; }
a { color: #3498db; }
</style>