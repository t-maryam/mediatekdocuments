# Application C# MediatekDocuments – Atelier 2

> Ce dépôt présente les fonctionnalités **ajoutées** dans le cadre de l'Atelier 2.
> Pour la présentation de l'application d'origine et de ses fonctionnalités initiales, consulter le dépôt d'origine :
> https://github.com/CNED-SLAM/MediaTekDocuments

## Présentation

MediatekDocuments est une application de bureau Windows (C# / WinForms / .NET Framework 4.7.2) permettant aux employés de la médiathèque MediaTek86 de gérer le catalogue de documents (livres, DVD, revues), leurs commandes et abonnements.
L'application exploite une API REST PHP pour accéder à la base de données MySQL `mediatek86`.
Le code de l'API se trouve ici : https://github.com/t-maryam/rest_mediatekdocuments

## Fonctionnalités ajoutées

### Authentification (Mission 4)

L'application démarre désormais sur une fenêtre de connexion. L'utilisateur doit saisir son login et son mot de passe. Selon le service d'appartenance, les droits d'accès sont différents :

- **Administrateur** et **service Administratif** : accès à toutes les fonctionnalités.
- **Service Prêts** : accès en consultation uniquement (livres, DVD, revues).
- **Service Culture** : accès refusé avec affichage d'un message.

Les mots de passe sont stockés en SHA256 dans la base de données.

### Gestion des commandes de livres et de DVD (Mission 2 – Tâche 1)

Deux nouveaux onglets permettent de gérer les commandes de livres et de DVD séparément.

Pour chaque onglet, il est possible de :
- Rechercher un document par son numéro et afficher ses informations.
- Consulter la liste de ses commandes triée par date (ordre inverse).
- Ajouter une nouvelle commande (id auto-généré, date, montant, nombre d'exemplaires).
- Modifier l'étape de suivi d'une commande parmi : en cours, relancée, livrée, réglée (avec règles métier : pas de retour arrière si livrée/réglée, pas de réglée si pas livrée).
- Supprimer une commande uniquement si elle n'est pas encore livrée.

Lorsqu'une commande passe à l'étape **livrée**, un trigger SQL génère automatiquement les exemplaires correspondants dans la BDD avec un numéro séquentiel, la date de commande comme date d'achat, et l'état "neuf".

### Gestion des commandes de revues / abonnements (Mission 2 – Tâche 2)

Un nouvel onglet **Abonnements Revues** permet de gérer les abonnements aux revues.

Il est possible de :
- Rechercher une revue par son numéro et afficher ses informations.
- Consulter la liste de ses abonnements triée par date (ordre inverse).
- Ajouter un nouvel abonnement (id auto-généré, date de commande, montant, date de fin).
- Renouveler un abonnement existant en modifiant uniquement la date de fin (doit être ultérieure à l'ancienne).
- Supprimer un abonnement uniquement si aucune parution n'y est rattachée (vérification via la méthode `ParutionDansAbonnement`).

Au démarrage de l'application, une fenêtre d'alerte s'affiche automatiquement pour rappeler les abonnements se terminant dans moins de 30 jours (visible uniquement pour les services Administratif et Administrateur).

### Sécurité et qualité (Mission 5)

- Les identifiants de connexion à l'API (login/pwd) sont désormais stockés dans `App.config` et non plus en dur dans le code.
- Les avertissements SonarQube ont été corrigés (règle S6562 : ajout du `DateTimeKind` dans les constructeurs `DateTime`).
- Les logs sont gérés avec **Serilog** : chaque erreur est enregistrée dans un fichier `logs/log.txt`.

### Déploiement (Mission 7)

- L'application peut être configurée pour pointer sur l'API en ligne (AwardSpace) via `App.config`.
- Un installeur ClickOnce est disponible dans le dossier `installer/` du dépôt.

## Installation et utilisation en local

### Prérequis

- Visual Studio 2022 Enterprise
- WampServer (avec MySQL 9.x et PHP 8.3)
- L'API REST `rest_mediatekdocuments` installée en local (voir son README)

### Étapes

1. Cloner ou télécharger ce dépôt et renommer le dossier en `mediatekdocuments`.
2. Ouvrir le fichier `MediaTekDocuments.sln` dans Visual Studio 2022.
3. Vérifier que les packages NuGet sont bien restaurés (clic droit sur la solution → Restaurer les packages NuGet).
4. Ouvrir `MediaTekDocuments/App.config` et vérifier les valeurs suivantes :
   - `uriApi` : URL de l'API (par défaut `http://localhost/rest_mediatekdocuments/`)
   - `apiLogin` et `apiPwd` : identifiants de connexion à l'API (par défaut `admin` / `adminpwd`)
5. Lancer WampServer et s'assurer que l'API REST est accessible.
6. Compiler et lancer l'application (F5 dans Visual Studio).
7. Se connecter avec un des comptes de test (mot de passe : `motdepasse`) :
   - `admin` → Administrateur (accès total)
   - `alice` → service Administratif (accès total)
   - `bob` → service Prêts (consultation uniquement)
   - `claire` → service Culture (accès refusé)

### Installation via l'installeur ClickOnce

1. Aller dans le dossier `installer/` du dépôt.
2. Double-cliquer sur `setup.exe`.
3. Suivre les instructions d'installation.
4. L'application s'installe et se lance automatiquement.


```
