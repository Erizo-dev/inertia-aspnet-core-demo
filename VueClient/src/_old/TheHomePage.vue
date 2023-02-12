<script setup lang="ts">
import { ref, reactive } from "vue"
import { Link, Head } from "@inertiajs/vue3"
import { router } from '@inertiajs/vue3'

const props = defineProps<{
    controller: {id: string},
    share: {'test-shared': string}
}>();
console.log("props", props)
const form = ref<HTMLFormElement>()
const loginForm = reactive({name:""})

function login(){
router.post('/login', loginForm, {
    onError: (err) => {
        console.log("error attempting to log in" , err)
    },
    onBefore: (e) => {
        console.log("sending login request", e)
    }
})
}

function submit() {
    router.post('/users', new FormData(form.value), {
        onSuccess: (e) => {
            console.log("success", e)
        },
        onBefore: (e) => {
            console.log("before sending", e)
        }
    })
}

</script>
<template>

    <Head>
        <title>The Home fage from Head</title>
        <meta name="description" content="Your page description">
    </Head>

    <h2>The home page</h2>
    <p>Props</p>
<div>
  <form @submit.prevent="login">
    <label for="name">Name:</label>
    <input id="name" v-model="loginForm.name" />
    <button type="submit">login</button>
  </form>
</div>
    <pre>{{ controller }}</pre>
    <pre>{{ share }}</pre>
    <p>available variables acessible through the $page magic variable</p>
    <pre>{{ $page }}</pre>
    <form @submit.prevent="submit" ref="form" class="basic-form">
        <div class="form-group">

            <label for="firstName">First name:</label>
            <input id="firstName" name="firstName²" />
        </div>

        <div class="form-group">

            <label for="lastName">Last name:</label>
            <input id="lastName" name="lastName" />
        </div>


        <div class="form-group">
            <label for="email">Email:</label>
            <input id="email" name="email" />
        </div>

        <div class="form-group">
            <button type="submit">Submit</button>
        </div>
    </form>
    <div class="make-scroll">
        <div><a href="/contact">contact with postback</a></div>
        <div>
            <Link href="/about">about no postback</Link>
        </div>
    </div>
</template>
<style>
.make-scroll {
    min-height: 150vh;
    display: grid;
    place-items: center;
    align-items: center;
    background-color: #ccc;
}

.basic-form {
    max-width: 50%;
    margin-inline: auto;
}

.form-group+.form-group {
    margin-top: 2em;
}

.form-group {
    display: flex;
    gap: 2em;
}

.form-group label,
input {
    flex: 1;
}
</style>