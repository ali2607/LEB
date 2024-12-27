# LEB

## Description du projet

**Les notes d'Elie Baba** est une application web novatrice de gestion de notes, permettant de stocker divers types de contenus (listes de courses, poèmes, recettes, etc.). Grâce à l’intégration d’une API basée sur un Large Language Model (LLM), les utilisateurs peuvent générer des résumés de leurs notes en un seul clic. De plus, une barre de recherche avancée, elle aussi propulsée par le LLM, facilite la création et la modification de notes, ainsi que la recherche d’informations précises au sein de centaines de contenus. Cette solution offre une expérience fluide et efficace pour gérer ses données.

## prérequis
- Docker desktop installé et mis à jour.

## Instructions de lancement du projet

1. **Télécharger le projet**  
   Clonez ou téléchargez ce dépôt sur votre machine locale.

2. **Aller sur le fichier `docker-compose.yml`**  
   Ouvrez le fichier `docker-compose.yml`.

3. **Ajouter sa clé API**  
   Dans les variables d’environnement (sections `environment:`), remplacez la valeur de `ApiKey` par votre propre clé API.  
   ```yaml
   environment:
     - ApiKeys__MyApiKey=VOTRE_CLE_API
   ```
4. **Sauvegarder les modifications**  
   Enregistrez et fermez le fichier docker-compose.yml.

5. **Lancer Docker Desktop**  
   lancez l'application Docker Desktop pour pouvoir executer les images.
      
6. **Construire l'image Docker**  
   Depuis votre terminal, à la racine du projet, exécutez la commande :
   ```bash
   docker-compose build
   ```
   
7. **Démarrer le projet**  
   Lancez le conteneur:
   ```bash
   docker-compose up
   ```
   - Le site se trouvera à l'url suivante http://localhost:8080/
   - l'interface Swagger de l'api se trouve à l'url suivante http://localhost:7292/swagger/index.html
