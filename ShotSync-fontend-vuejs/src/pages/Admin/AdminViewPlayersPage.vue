<template>
  <q-page class="q-pa-md">
    <q-table title="Players List" :rows="rows" :columns="columns" row-key="id" flat bordered>
      <template v-slot:body-cell-actions="props">
        <q-td align="center">
          <q-btn flat round icon="edit" color="primary" @click="editPlayer(props.row)" />
          <q-btn flat round icon="delete" color="negative" @click="deletePlayer(props.row)" />
        </q-td>
      </template>
    </q-table>
  </q-page>
</template>

<script setup lang="ts">
import { ref } from 'vue'
import type { QTableColumn } from 'quasar'

interface Player {
  id: number
  index: number
  name: string
  created_at: string
  status: string
  score: number
  rank: string
}

const columns: QTableColumn<Player>[] = [
  { name: 'index', label: 'No.', align: 'left', field: (row) => row.index },
  { name: 'name', label: 'Player Name', align: 'left', field: 'name' },
  { name: 'created_at', label: 'Created At', align: 'left', field: 'created_at' },
  { name: 'status', label: 'Status', align: 'left', field: 'status' },
  { name: 'score', label: 'Score', align: 'left', field: 'score' },
  { name: 'rank', label: 'Rank', align: 'left', field: 'rank' },
  { name: 'actions', label: 'Actions', align: 'center', field: 'id' }, // dummy field
]

const rows = ref<Player[]>([
  {
    id: 1,
    index: 1,
    name: 'Player One',
    created_at: '2024-05-01',
    status: 'Active',
    score: 95,
    rank: 'Gold',
  },
  {
    id: 2,
    index: 2,
    name: 'Player Two',
    created_at: '2024-04-15',
    status: 'Inactive',
    score: 78,
    rank: 'Silver',
  },
  {
    id: 3,
    index: 3,
    name: 'Player Three',
    created_at: '2024-03-22',
    status: 'Active',
    score: 88,
    rank: 'Platinum',
  },
])

function editPlayer(row: Player) {
  console.log('Edit player:', row)
}

function deletePlayer(row: Player) {
  console.log('Delete player:', row)
}
</script>
