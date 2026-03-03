# ANALYSE.md — Système de Gestion Hospitalière

## Étape 1 — Modélisation Initial

### Comment garantissez-vous l'unicité du numéro de dossier ?
L'unicité est garantie au niveau de la **base de données** via un index unique configuré dans `OnModelCreating`

`HasIndex().IsUnique()` crée une contrainte réelle en base de données. Si deux requêtes simultanées tentaient d'insérer le même numéro de dossier, la base de données rejetterait la seconde même si la validation applicative avait passé.

### Quelle stratégie utilisez-vous pour les clés primaires ?
On utilise un `int` auto-incrémenté généré par la base de données (`DatabaseGeneratedOption.Identity`) 

**Avantages** : simple, performant, lisible, optimal pour les jointures et les index.  

### Comment validez-vous la date de naissance ?
Double validation complémentaire :

**1. Côté applicatif** — Custom Data Annotation `PastDateAttribute` :

Vérifie que `date.Date < DateTime.Today` avant d'aller en base de données.

**2. Côté base de données** — Contrainte `CHECK` dans `OnModelCreating` 
Garantit l'intégrité même si quelqu'un insère directement en base sans passer par l'application.

## Étape 2 — Intégration des médecins

### Quelle relation entre Doctor et Department ?
Relation Many-to-One : un médecin appartient à un seul département, un département peut avoir plusieurs médecins.

Pour le responsable médical, relation One-to-One entre `Department` et `Doctor` :

### Quel DeleteBehavior ? Justifiez votre choix.

**`DeleteBehavior.Restrict` pour Doctor → Department** :  
On ne peut pas supprimer un département qui a encore des médecins. Cela oblige à réaffecter les médecins avant toute suppression.

**`DeleteBehavior.SetNull` pour HeadDoctor → Department** :  
Si le médecin responsable est supprimé, `HeadDoctorId` passe à `null` sans bloquer l'opération. Le département existe toujours, il n'a juste plus de responsable désigné. 

## Étape 5 — Chargement des données liées

### Eager Loading vs Lazy Loading pour ces cas d'usage
| | Eager Loading | Lazy Loading |
|---|---|---|
| Requêtes SQL | 1 requête avec jointures | 1 + N requêtes |
| Cas d'usage | On sait à l'avance ce qu'on charge | Accès imprévisible aux données |
| Risque | Sur-chargement si trop de `Include` | Problème N+1 si mal utilisé |
| Notre choix | ✅ Tableaux de bord | ❌ Non adapté ici |

Pour nos tableaux de bord, **Eager Loading est clairement préférable** car on connaît exactement les données nécessaires à l'affichage.

### Quand est-il pertinent d'utiliser des projections ?
Les projections (`Select`) sont pertinentes quand :
- On n'a besoin que d'un sous-ensemble des champs d'une entité
- On veut agréger des données (Count, Sum, Average)
- On veut éviter de charger des entités entières en mémoire

Exemple dans les stats département : au lieu de charger tous les médecins et toutes les consultations, on ne récupère que les compteurs directement en SQL.

### Quel impact sur les performances si on charge toutes les données sans filtre ?
Avec 100 000 patients, charger tous les patients avec toutes leurs consultations et tous leurs médecins sans filtre ni pagination serait catastrophique :
- Consommation mémoire exponentielle
- Requêtes SQL très lentes (jointures sur des millions de lignes)
- Timeout des requêtes HTTP

Solution : toujours paginer (`Skip/Take`), filtrer avant de charger (`Where` avant `Include`), et utiliser `AsNoTracking()` sur les lectures.