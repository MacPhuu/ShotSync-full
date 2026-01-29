<template>
  <q-page class="row justify-center">
    <div class="col justify-center items-center" v-if="loading">
      <div class="row justify-center items-center" style="height: 60vh">
        <q-spinner-cube color="orange" size="5.5em" />
      </div>
    </div>
    <div class="column col" v-if="!loading">
      <div
        class="event-infor-component w-100 bg-no-2 q-my-sm row"
        style="height: auto; border-radius: 20px; width: 100%"
      >
        <div class="col column justify-center items-center">
          <div
            class="row bg-primary text-white text-h6 justify-center items-center relative-position"
            style="height: 40px; width: 60%; min-width: 500px; border-radius: 0 0 10px 10px"
          >
            PLAYER LIST
            <div v-if="isHost" class="absolute-left q-ml-sm row q-gutter-x-sm items-center full-height">
                <q-btn
                    rounded
                    icon="play_arrow"
                    label="Start"
                    color="positive"
                    size="xs"
                    @click="handleStartTournament"
                >
                    <q-tooltip>Start Tournament</q-tooltip>
                </q-btn>
                <q-btn
                    rounded
                    icon="auto_fix_high"
                    label="Seed & Start"
                    color="secondary"
                    size="xs"
                    @click="handleSeedAndStart"
                >
                    <q-tooltip>Seed 16 & Start</q-tooltip>
                </q-btn>
                <q-btn
                    rounded
                    icon="restart_alt"
                    label="Reset"
                    color="negative"
                    size="xs"
                    @click="handleResetTournament"
                >
                    <q-tooltip>Clear Bracket & Reset</q-tooltip>
                </q-btn>
            </div>
            <q-btn
                v-if="isHost"
                icon="add"
                round
                dense
                flat
                size="sm"
                class="absolute-right q-mr-sm"
                @click="showAddPlayerDialog = true"
            >
                <q-tooltip>Add Player by Email</q-tooltip>
            </q-btn>
          </div>
          <div class="row q-my-lg justify-center bg-no-2" style="width: 95%">
            <PlayerCardComponent
              v-for="(player, index) in players"
              :key="index"
              :playerName="player.name"
              :playerNation="player.nation"
              :playerPortrait="player.portrait || defaultPortrait"
            />
          </div>
        </div>
      </div>
    </div>

    <!-- Add Player Dialog -->
    <q-dialog v-model="showAddPlayerDialog">
        <q-card style="min-width: 350px">
            <q-card-section>
                <div class="text-h6">Register Player</div>
            </q-card-section>

            <q-card-section class="q-pt-none">
                <q-input dense v-model="playerEmail" label="Player Email" autofocus @keyup.enter="handleAddPlayer" />
            </q-card-section>

            <q-card-actions align="right" class="text-primary">
                <q-btn flat label="Cancel" v-close-popup />
                <q-btn flat label="Add" @click="handleAddPlayer" />
            </q-card-actions>
        </q-card>
    </q-dialog>

  </q-page>
</template>

<script setup lang="ts">
import PlayerCardComponent from 'components/PlayerCardComponent.vue'
import { ref, onMounted } from 'vue'
import { useRoute } from 'vue-router'
import { useQuasar } from 'quasar'
import type { Player, Event } from '../../components/models'
import EventService from 'src/services/event.service'
import PlayerService from 'src/services/player.service'
import { useEventStore } from 'src/stores/event'

const $q = useQuasar()
const route = useRoute()

const eventStore = useEventStore()

const eventName = route.params.eventName
const eventNameStr = (Array.isArray(eventName) ? eventName[0] : eventName || 'error') as string

const loading = ref(true)

// Add Player Dialog State
const showAddPlayerDialog = ref(false)
const playerEmail = ref('')

const defaultPortrait =
  'https://storage.googleapis.com/wnt-cm-public/media/players/generic-profile_mini_dcryfs.webp'

const players = ref<Player[]>([])
const event = ref<Event>({
  id: 0,
  name: '',
  venue: '',
  location: '',
  date: '',
  isHappen: false,
})

const fetchData = async () => {
  try {
    const events = await EventService.getEventsByName(eventNameStr);

    if (events && events.length > 0) {
      const e = events[0]!
      event.value = {
        id: e.id,
        name: e.name || '',
        venue: e.venue || '',
        location: e.location || '',
        date: e.date,
        isHappen: e.isHappen
      }

      // NOW fetch players for this event
      const playersForEvent = await PlayerService.getPlayersByEvent(e.id);
      players.value = playersForEvent.map((p) => ({
        id: p.id,
        name: p.name || '',
        nation: p.nation || '',
        portrait: p.portrait || defaultPortrait,
        point: p.point || '0',
      })) as Player[]
    }
  } catch (error) {
    console.error('Error fetching data:', error)
  } finally {
    loading.value = false
  }
}

// Check if user is host (Role 2)
const isHost = (localStorage.getItem('role') === '2') || (sessionStorage.getItem('role') === '2');

const handleAddPlayer = async () => {
    try {
        if (!event.value.id) return;

        await EventService.registerPlayerByEmail(event.value.id, playerEmail.value);

        $q.notify({
            type: 'positive',
            message: 'Player registered successfully!'
        });

        playerEmail.value = '';
        showAddPlayerDialog.value = false;

        // Refresh data
        await fetchData();

    } catch (error) {
        const err = error as Error;
        $q.notify({
            type: 'negative',
            message: err.message || 'Failed to register player.'
        });
    }
}

const handleStartTournament = async () => {
  try {
    await EventService.startTournament(event.value.id)
    $q.notify({
      type: 'positive',
      message: 'Tournament started! Bracket generated.'
    })
    await fetchData()
  } catch (error) {
    const err = error as { response?: { data?: { message?: string } } };
    $q.notify({
      type: 'negative',
      message: err.response?.data?.message || 'Failed to start tournament'
    })
  }
}

const handleSeedAndStart = async () => {
  try {
    $q.loading.show({ message: 'Filling players to power of 2 and generating bracket...' })

    // 1. Fill data (Add enough players to reach next power of 2)
    await EventService.fillPlayers(event.value.id)

    // 2. Start
    await EventService.startTournament(event.value.id)

    $q.notify({
      type: 'positive',
      message: 'Success! Tournament is ready.'
    })

    await fetchData()
  } catch (error) {
    const err = error as { response?: { data?: { message?: string } } };
    $q.notify({
      type: 'negative',
      message: err.response?.data?.message || 'Failed to seed/start tournament'
    })
  } finally {
    $q.loading.hide()
  }
}

const handleResetTournament = async () => {
    try {
        if (!confirm('Are you sure you want to reset the tournament? This will delete all matches.')) return;

        $q.loading.show({ message: 'Resetting tournament...' })
        await EventService.resetTournament(event.value.id)

        $q.notify({ type: 'positive', message: 'Tournament reset successfully.' })
        await fetchData()
    } catch (error) {
        const err = error as { response?: { data?: { message?: string } } };
        $q.notify({
            type: 'negative',
            message: err.response?.data?.message || 'Failed to reset tournament'
        })
    } finally {
        $q.loading.hide()
    }
}

onMounted(async () => {
  console.log(eventStore.getEventId)
  await fetchData()
})
</script>
