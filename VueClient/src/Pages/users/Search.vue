<template>
  <div>
    <h1 class="mb-8 text-3xl font-bold">Users</h1>
    <div class="flex items-center justify-between mb-6">
      <search-filter :max-width="300" v-model="form.search" class="mr-4 w-full max-w-md" @reset="reset">
        <label class="block text-gray-700">Role:</label>
        <select v-model="form.role" class="form-select mt-1 w-full">
          <option :value="null" />
          <option value="user">User</option>
          <option value="owner">Owner</option>
        </select>
        <label class="block mt-4 text-gray-700">Trashed:</label>
        <select v-model="form.trashed" class="form-select mt-1 w-full">
          <option :value="null" />
          <option value="with">With Trashed</option>
          <option value="only">Only Trashed</option>
        </select>
      </search-filter>
      <Link class="btn-indigo" :href="`/users/create`">
      <span>Create</span>
      <span class="hidden md:inline">&nbsp;User</span>
      </Link>
    </div>
    <div class="bg-white rounded-md shadow overflow-x-auto">
      <table class="w-full whitespace-nowrap">
        <tr class="text-left font-bold">
          <th class="pb-4 pt-6 px-6">Name</th>
          <th class="pb-4 pt-6 px-6">Email</th>
          <th class="pb-4 pt-6 px-6" colspan="2">Role</th>
        </tr>
        <tr v-for="user in users?.data" :key="user.id" class="hover:bg-gray-100 focus-within:bg-gray-100">
          <td class="border-t">
            <Link class="flex items-center px-6 py-4 focus:text-indigo-500" :href="`/users/edit/${user.id}`">
            <img v-if="user.photoPath" class="block -my-2 mr-2 w-5 h-5 rounded-full" :src="user.photoPath" />
            {{ user.lastName }}&nbsp;{{ user.firstName }}
            <icon v-if="user.isDeleted" name="trash" class="flex-shrink-0 ml-2 w-3 h-3 fill-gray-400" />
            </Link>
          </td>
          <td class="border-t">
            <Link class="flex items-center px-6 py-4" :href="`/users/edit/${user.id}`" tabindex="-1">
            {{ user.email }}
            </Link>
          </td>
          <td class="border-t">
            <Link class="flex items-center px-6 py-4" :href="`/users/edit/${user.id}`" tabindex="-1">
            {{ user.owner ? 'Owner' : 'User' }}
            </Link>
          </td>
          <td class="w-px border-t">
            <Link class="flex items-center px-4" :href="`/users/edit/${user.id}`" tabindex="-1">
            <icon name="cheveron-right" class="block w-6 h-6 fill-gray-400" />
            </Link>
          </td>
        </tr>
        <tr v-if="users.data.length === 0">
          <td class="px-6 py-4 border-t" colspan="4">No users found.</td>
        </tr>
      </table>
    </div>
  </div>
</template>

<script lang="ts" setup>
import { computed, defineComponent, PropType, reactive } from 'vue'
import { Link, router, useForm } from '@inertiajs/vue3'
import { Link as LinkModel, LinksDto, SearchFilters, User } from '../../models/models'
import Icon from '../../common/Icon.vue'
import SearchFilter from '../../common/SearchFilter.vue'
import { watchDebounced } from '@vueuse/core'

type UserSearch = {
  data: User[],
  links: LinksDto,
}
const props = defineProps<{
  filters: SearchFilters,
  users: UserSearch
}>();

const form = reactive({
  search: props.filters.search || '',
  role: props.filters.role || '',
  trashed: props.filters.trashed || false
})

const reset = () => {
  form.search = '';
    form.trashed = '';
    form.role = '';
}
const updateSearch = (value: string) => {
  console.log("updating search with ", value)
  form.search = value
}
const url = computed(() => {
  let path = "/users"
  let params = []
  if (form.search) {
    params.push(`search=${form.search}`)
  }
  if (form.trashed) {
    params.push(`trashed=${form.trashed}`)
  }
  if (form.role) {
    params.push(`role=${form.role}`)
  }
  if (params) {
    path = `${path}?${params.join('&')}`
  }



  console.log("New search url", path)
  return path;

})
watchDebounced([form, url], () => {
  router.get(url.value)
}, { deep: true, debounce: 300 })

const pageLinks = computed(() => {
  let links: LinkModel[] = [];
  links.push({
    label: 'first',
    url: '/users?page=1&perPage=10',
    active: !props.users.links.isLastPage
  })
  links.push({
    label: 'last',
    url: `/users?page=${1}&perPage=10`,
    active: !props.users.links.isLastPage
  })
  links.push({
    label: 'previous',
    url: `/users?page=${props.users.links.pageNumber - 1}&perPage=10`,
    active: props.users.links.hasPreviousPage
  })
  links.push({
    label: 'next',
    url: `/users?page=${props.users.links.pageNumber + 1}&perPage=10`,
    active: !props.users.links.hasNextPage
  })
  return links

})

</script>
