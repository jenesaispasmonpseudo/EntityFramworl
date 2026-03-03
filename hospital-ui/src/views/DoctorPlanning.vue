<template>
  <div>
    <h1>Planning Médecin</h1>

    <div class="selector">
      <label>ID du médecin :</label>
      <input v-model="doctorId" type="number" min="1" />
      <button @click="loadPlanning">Charger</button>
    </div>

    <div v-if="planning">
      <div class="info-card">
        <h2>{{ planning.fullName }}</h2>
        <p><strong>Spécialité :</strong> {{ planning.specialty }}</p>
        <p><strong>Département :</strong> {{ planning.departmentName }}</p>
      </div>

      <h3>Consultations à venir</h3>
      <table v-if="planning.upcomingConsultations.length > 0">
        <thead>
          <tr>
            <th>Date</th>
            <th>Patient</th>
            <th>Statut</th>
            <th>Notes</th>
          </tr>
        </thead>
        <tbody>
          <tr v-for="c in planning.upcomingConsultations" :key="c.id">
            <td>{{ formatDate(c.date) }}</td>
            <td>{{ c.patientFullName }}</td>
            <td>{{ c.status }}</td>
            <td>{{ c.notes || '-' }}</td>
          </tr>
        </tbody>
      </table>
      <p v-else>Aucune consultation à venir.</p>
    </div>

    <p v-else-if="searched">Médecin introuvable.</p>
  </div>
</template>

<script>
import api from '../api/api'

export default {
  data() {
    return {
      doctorId: this.$route.params.id || 1,
      planning: null,
      searched: false
    }
  },
  mounted() {
    this.loadPlanning()
  },
  methods: {
    async loadPlanning() {
      this.searched = false
      try {
        const response = await api.get(`/Dashboard/doctor/${this.doctorId}/planning`)
        this.planning = response.data
      } catch (e) {
        this.planning = null
      } finally {
        this.searched = true
      }
    },
    formatDate(date) {
      return new Date(date).toLocaleDateString('fr-FR', {
        day: '2-digit', month: '2-digit', year: 'numeric',
        hour: '2-digit', minute: '2-digit'
      })
    }
  }
}
</script>

<style scoped>
table { width: 100%; border-collapse: collapse; margin-top: 1rem; }
th, td { padding: 0.75rem; border: 1px solid #ddd; text-align: left; }
th { background: #2c3e50; color: white; }
.info-card { background: #f8f9fa; padding: 1rem; border-radius: 8px; margin: 1rem 0; }
.selector { display: flex; align-items: center; gap: 1rem; margin-bottom: 2rem; }
input { padding: 0.5rem; border: 1px solid #ddd; border-radius: 4px; width: 80px; }
button {
  padding: 0.5rem 1rem; background: #2c3e50;
  color: white; border: none; border-radius: 4px; cursor: pointer;
}
</style>