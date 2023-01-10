<script lang="ts">
    import type { PageData } from "./$types";
	import { invalidateAll } from "$app/navigation";

    export let data: PageData;

    let pizza = {
        Name: "",
        IsGlutenFree: false
    }
    let result: String;

    async function doPost(){
        const res = await fetch('/api/Pizza', {
            method: 'POST',
            headers: {
                'Accept': 'application/json',
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(pizza)
        });

        if (res.status === 200) {
            await invalidateAll();

            pizza.Name = "";
            pizza.IsGlutenFree = false;
        }
    }
</script>

<form method="POST" action="/api/Pizza" on:submit|preventDefault={doPost}>
    <label>
        Name
        <input name="name" type="text" bind:value={pizza.Name}>
    </label>
    <label>
        isGlutenFree
        <input name="isGlutenFree" type="checkbox" bind:checked={pizza.IsGlutenFree}>
    </label>
    <button type="submit">Submit</button>
</form>

{result}
{pizza.IsGlutenFree}

{#each data.post.pizzas as pizza}
    <h2>{pizza.name}</h2>
    <h3>Is Gluten Free: {pizza.isGlutenFree}</h3>
    <h4>ID: {pizza.id}</h4>
{/each}
